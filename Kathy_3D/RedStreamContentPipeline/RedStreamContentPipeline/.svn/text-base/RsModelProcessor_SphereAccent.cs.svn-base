using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
namespace RedStreamContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "RedStreamContentPipeline.RsModelProcessor_SphereAccent")]
    public class RsModelProcessor_SphereAccent : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            GenerateTangentFramesRecursive(input);
            return base.Process(input, context);
        }
        static IList<string> acceptableVertexChannelNames = new string[]
        {
            VertexChannelNames.TextureCoordinate(0),
            VertexChannelNames.Normal(0),
            VertexChannelNames.Binormal(0),
            VertexChannelNames.Tangent(0),
        };
        private void GenerateTangentFramesRecursive(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;
            Vector3 min = new Vector3(), max = new Vector3();
            //int vCount = 0;
            foreach (GeometryContent batch in mesh.Geometry)
                foreach (Vector3 v in batch.Vertices.Positions)
                {
                    min = Vector3.Min(min, v);
                    max = Vector3.Max(max, v);
                }
            if (mesh != null)
            {
                MeshHelper.CalculateNormals(mesh, true);
                MeshHelper.CalculateTangentFrames(mesh,
                    VertexChannelNames.TextureCoordinate(0),
                    VertexChannelNames.Tangent(0),
                    VertexChannelNames.Binormal(0));
            }
            //MeshHelper.TransformScene(node, mesh.AbsoluteTransform);
            MeshHelper.TransformScene(node, Matrix.CreateTranslation(-(min + max) * .5f));
            MeshHelper.TransformScene(node, Matrix.CreateRotationX(MathHelper.ToRadians(-90.0f)));
            foreach (NodeContent child in node.Children)
            {
                GenerateTangentFramesRecursive(child);
            }
        }
        protected override void ProcessVertexChannel(GeometryContent geometry, int vertexChannelIndex, ContentProcessorContext context)
        {
            String vertexChannelName = geometry.Vertices.Channels[vertexChannelIndex].Name;
            //if this vertex channel has an acceptable names, process it as normal.
            if (acceptableVertexChannelNames.Contains(vertexChannelName))
            {
                base.ProcessVertexChannel(geometry, vertexChannelIndex, context);
            }
            //otherwise, remove it from the vertex channels; it's just extra data
            //we don't need.
            else
            {
                geometry.Vertices.Channels.Remove(vertexChannelName);
            }
        }
    }
}
