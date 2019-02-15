using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{

    public BlockInfo.BlockID blockid;
    
    public Vector3 pos;
    public bool IsTransparent;

    public static Block Nothing = new Block(new Chuck(), BlockInfo.BlockID.Empty,false, Vector3.zero);
    public static Block Dirt = new Block(new Chuck(), BlockInfo.BlockID.Dirt, false, Vector3.zero);

    public BlockInfo.BlockID ID
    {
        get { return blockid; }
        set { blockid = value;chuck.Blocks[(int)pos.x, (int)pos.y , (int)pos.z].blockid = value; chuck.PresentImmediately();}
    }
    public Chuck chuck;
    public Map map;

    public Block(Chuck parent, BlockInfo.BlockID ID,bool Transparent, Vector3 Pos)
    {
        blockid =ID;
        pos = Pos;
        IsTransparent = Transparent;
        chuck = parent;
        map = parent.map;

    }

    public Block(Chuck parent,BlockInfo info,Vector3 Pos)
    {
        blockid = info.id;
        pos = Pos;
        IsTransparent = info.IsTransparent;
        chuck = parent;
        map = parent.map;
    }

    private void Parent_OnDestory()
    {

    }

    private void Parent_OnBuild()
    {

    }

    public void Present()
    {
        bool HasLeft=false, HasRight=false, HasTop=false, HasBottom=false, HasFront=false, HasBehind=false;
        if(ID!=BlockInfo.BlockID.Empty)
        {
            int Wx=(int)(pos.x+chuck.Index.x*16), Wy=(int)pos.y,Wz = (int)(pos.z + chuck.Index.y * 16);
            if(pos.x>0&&pos.x<15&&pos.z>0&&pos.z<15&&pos.y>0&&pos.y<254)
            {
                if (chuck.Blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z]==null||chuck.Blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z].IsTransparent)HasLeft = true;
                if (chuck.Blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z]==null||chuck.Blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z].IsTransparent)HasRight = true;
                if (chuck.Blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z]==null||chuck.Blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z].IsTransparent) HasBottom = true;
                if (chuck.Blocks[(int)pos.x, (int)pos.y + 1, (int)pos.z]==null||chuck.Blocks[(int)pos.x, (int)pos.y + 1, (int)pos.z].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x, (int)pos.y + 1, (int)pos.z].IsTransparent) HasTop = true;
                if (chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1]==null||chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1].IsTransparent) HasFront = true;
                if (chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1]==null||chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1].ID == BlockInfo.BlockID.Empty || chuck.Blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1].IsTransparent) HasBehind = true;
            }
            else
            {
                //Debug.Log(Wx + " Chuck " + Wz + " " + Wy);
                if (map[Wx - 1, Wy, Wz].ID == BlockInfo.BlockID.Empty || map[Wx - 1, Wy, Wz].IsTransparent) HasLeft = true;
                if (map[Wx + 1, Wy, Wz].ID == BlockInfo.BlockID.Empty || map[Wx + 1, Wy, Wz].IsTransparent) HasRight = true;
                if (map[Wx, Wy - 1, Wz].ID == BlockInfo.BlockID.Empty || map[Wx, Wy - 1, Wz].IsTransparent) HasBottom = true;
                if (map[Wx, Wy + 1, Wz].ID == BlockInfo.BlockID.Empty || map[Wx, Wy + 1, Wz].IsTransparent) HasTop = true;
                if (map[Wx, Wy, Wz - 1].ID == BlockInfo.BlockID.Empty || map[Wx, Wy, Wz - 1].IsTransparent) HasFront = true;
                if (map[Wx, Wy, Wz + 1].ID == BlockInfo.BlockID.Empty || map[Wx, Wy, Wz + 1].IsTransparent) HasBehind = true;

            }

            Vector2[] UV = CubeConverter.UV[ID];
            CubeConverter.AddCubeMesh(chuck.Vectexes, chuck.TextureUV, chuck.Triangles, pos.x, pos.y, pos.z, 1, UV[0], UV[1], UV[2], UV[3], UV[4], UV[5], HasLeft, HasRight,HasBottom, HasTop, HasFront, HasBehind);
        }

    }

}
public class BlockInfo
{
    public enum BlockID { Empty = 0, Dirt }
    //public enum BlockTransparentMode {None=0,TransparentOnly,TransparentAndCut}
    public bool IsTransparent;
    public BlockID id;
}

