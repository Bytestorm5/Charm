﻿
namespace Tiger.Schema.Entity;

public class EntitySkeleton : EntityResource
{
    public EntitySkeleton(FileHash resource) : base(resource)
    {
    }

    public List<BoneNode> GetBoneNodes()
    {
        var nodes = new List<BoneNode>();
        D2Class_DE818080 skelInfo = (D2Class_DE818080)_tag.Unk18.Value;
        for (int i = 0; i < skelInfo.NodeHierarchy.Count; i++)
        {
            BoneNode node = new BoneNode();
            node.ParentNodeIndex = skelInfo.NodeHierarchy[i].ParentNodeIndex;
            node.Hash = skelInfo.NodeHierarchy[i].NodeHash;
            node.DefaultObjectSpaceTransform = new ObjectSpaceTransform
            {
                QuaternionRotation = skelInfo.DefaultObjectSpaceTransforms[i].Rotation,
                Translation = skelInfo.DefaultObjectSpaceTransforms[i].Translation.ToVec3(),
                Scale = skelInfo.DefaultObjectSpaceTransforms[i].Translation.W
            };
            node.DefaultInverseObjectSpaceTransform = new ObjectSpaceTransform
            {
                QuaternionRotation = skelInfo.DefaultInverseObjectSpaceTransforms[i].Rotation,
                Translation = skelInfo.DefaultInverseObjectSpaceTransforms[i].Translation.ToVec3(),
                Scale = skelInfo.DefaultInverseObjectSpaceTransforms[i].Translation.W
            };
            nodes.Add(node);
        }
        return nodes;
    }
}


public struct ObjectSpaceTransform
{
    public Vector4 QuaternionRotation;
    public Vector3 Translation;
    public float Scale;
}
public struct BoneNode
{
    public ObjectSpaceTransform DefaultObjectSpaceTransform;
    public ObjectSpaceTransform DefaultInverseObjectSpaceTransform;
    public int ParentNodeIndex;
    public TigerHash Hash;
}