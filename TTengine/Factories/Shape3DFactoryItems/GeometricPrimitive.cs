#region File Description
//-----------------------------------------------------------------------------
// GeometricPrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;
#endregion

namespace TTengine.Factories.Shape3DFactoryItems
{
    /// <summary>
    /// Base class for simple geometric primitive models. This provides a vertex
    /// buffer, an index buffer, plus methods for drawing the model. Classes for
    /// specific types of primitive (CubePrimitive, SpherePrimitive, etc.) are
    /// derived from this common base, and use the AddVertex and AddIndex methods
    /// to specify their geometry.
    /// 
    /// This class is borrowed from the Primitives3D sample.
    /// </summary>
    public abstract class GeometricPrimitive : IDisposable
    {
        #region Fields


        // During the process of constructing a primitive model, vertex
        // and index data is stored on the CPU in these managed lists.
        List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
        List<ushort> indices = new List<ushort>();


        // Once all the geometry has been specified, the InitializePrimitive
        // method copies the vertex and index data into these buffers, which
        // store it on the GPU ready for efficient rendering.
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect fx;


        #endregion

        #region Initialization


        /// <summary>
        /// Adds a new vertex to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            vertices.Add(new VertexPositionNormalTexture(position, normal, texCoord));
        }


        /// <summary>
        /// Adds a new index to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            indices.Add((ushort)index);
        }


        /// <summary>
        /// Queries the index of the current vertex. This starts at
        /// zero, and increments every time AddVertex is called.
        /// </summary>
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }


        /// <summary>
        /// Once all the geometry has been specified by calling AddVertex and AddIndex,
        /// this method copies the vertex and index data into GPU format buffers, ready
        /// for efficient rendering.
        protected void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
            // Create a vertex declaration, describing the format of our vertex data.
            // see http://virtuallyprogramming.com/Fundamentals/VerticesAndIndicies.html

            // Create a vertex buffer, and copy our vertex data into it.
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture),
                                            vertices.Count, BufferUsage.None);

            vertexBuffer.SetData(vertices.ToArray());

            // Create an index buffer, and copy our index data into it.
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                          indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());

            // Create a BasicEffect, which will be used to render the primitive.
            fx = new BasicEffect(graphicsDevice) {
                Alpha = 1f,
                PreferPerPixelLighting = false,
                TextureEnabled = true,
                Texture = TTGame.Instance.Content.Load<Texture2D>("earth8k"), // https://developer.xamarin.com/guides/cross-platform/game_development/monogame/3d/part2/
                LightingEnabled = true, // http://www.gamefromscratch.com/post/2015/08/20/Monogame-Tutorial-Beginning-3D-Programming.aspx
                FogEnabled = false
            };
            fx.EnableDefaultLighting();
            

        }


    /// <summary>
    /// Finalizer.
    /// </summary>
    ~GeometricPrimitive()
        {
            Dispose(false);
        }


        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();

                if (indexBuffer != null)
                    indexBuffer.Dispose();

                if (fx != null)
                    fx.Dispose();
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the primitive model, using the specified effect. Unlike the other
        /// Draw overload where you just specify the world/view/projection matrices
        /// and color, this method does not set any renderstates, so you must make
        /// sure all states are set to sensible values before you call it.
        /// </summary>
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;            
            int primitiveCount = indices.Count / 3;

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                //graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Count, 0, primitiveCount);
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, primitiveCount);
            }
        }


        /// <summary>
        /// Draws the primitive model, using a BasicEffect shader with default
        /// lighting. Unlike the other Draw overload where you specify a custom
        /// effect, this method sets important renderstates to sensible values
        /// for 3D model rendering, so you do not need to set these states before
        /// you call it.
        /// </summary>
        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            // Set BasicEffect parameters.
            fx.World = world; // * Matrix.CreateRotationY(-MathHelper.PiOver2);
            fx.View = view;
            fx.Projection = projection;
            fx.DiffuseColor = color.ToVector3();
            fx.Alpha = color.A / 255.0f;

            GraphicsDevice device = fx.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            //device.SamplerStates[1].AddressU = TextureAddressMode.Wrap;
            //device.SamplerStates[1].AddressV = TextureAddressMode.Wrap;

            if (color.A < 255)
            {
                // Set renderstates for alpha blended rendering.
                device.BlendState = BlendState.AlphaBlend;
            }
            else
            {
                // Set renderstates for opaque rendering.
                device.BlendState = BlendState.Opaque;
            }

            // Draw the model, using BasicEffect.
            Draw(fx);
        }


        #endregion
    }
}
