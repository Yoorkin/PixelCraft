using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public GameObject Player;
    public float LastTime = -10;
    public Dictionary<Vector2, Chuck> ChuckStream = new Dictionary<Vector2, Chuck>();
    public Material material;
    public Block this[int x, int y, int z]
    {
        get
        {

            //if (y >= 0 && y < 255)
            //{
            //    Vector2 Index = new Vector2(x / 16, z / 16);
            //    if (!ChuckStream.ContainsKey(Index)) LoadChuck(Index);
            //    Block block = ChuckStream[Index].Blocks[Mathf.Abs(x % 16), y, Mathf.Abs(z % 16)];
            //    if (block != null)
            //        return block;
            //    else
            //        return Block.Nothing;
            //}
            //else
            //{
            //   return Block.Nothing;
            //}
            return Block.Dirt;
        }
        set
        {
            if (y > 0 && y < 255)
            {
                Vector2 Index = new Vector2(x / 16, z / 16);
                if (!ChuckStream.ContainsKey(Index)) LoadChuck(Index);
                ChuckStream[Index].Blocks[x % 16, y, z % 16] = value;
            }
        }
    }

    private void Update()
    {
        //if(Time.time-LastTime>3)
        //{

            Vector2 PlayerIndex = new Vector2(Player.transform.position.x / 16, Player.transform.position.z / 16);
            for (int x = (int)PlayerIndex.x - 5; x < (int)PlayerIndex.x + 5; x++)
            {
                for (int y = (int)PlayerIndex.y - 5; y < (int)PlayerIndex.y + 5; y++)
                {
                    Vector2 index = new Vector2(x, y);
                    if (!ChuckStream.ContainsKey(index)) LoadChuck(index);
                    if(!ChuckStream[index].HasPresent)ChuckStream[index].Present();
                }
            }
            LastTime = Time.time;
        //}
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
    }
    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start () {
        CubeConverter.Init();
        //LoadChuck(new Vector2(10, 10));
        //ChuckStream[new Vector2(10, 10)].Present();
        //LoadChuck(new Vector2(10, 11));
        //ChuckStream[new Vector2(10, 11)].Present();

    }
	public void LoadChuck(Vector2 Index)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("Chuck"),new Vector3(Index.x*16,0,Index.y*16), Quaternion.identity);
        Chuck chuck = g.GetComponent<Chuck>();
        chuck.Load(this,Index, material);
        ChuckStream.Add(Index, chuck);
    }
}
public static class CubeConverter
{
    public static Dictionary<BlockInfo.BlockID, Vector2[]> UV = new Dictionary<BlockInfo.BlockID, Vector2[]>();
    public static void Init()
    {
        UV.Add(BlockInfo.BlockID.Dirt, AddUV(0,0,0,0,3,0,3,0,3,0,3,0));
    }
    private static Vector2[] AddUV(int Topx,int Topy,int Bottomx,int Bottomy,int Frontx,int Fronty,int Rightx,float Righty,float Behindx,float Behindy,float Leftx,float Lefty)
    {
        return new Vector2[] { new Vector2(Topx,Topy),new Vector2(Bottomx, Bottomy), new Vector2(Frontx, Fronty), new Vector2(Rightx, Righty), new Vector2(Behindx, Behindy), new Vector2(Leftx, Lefty) };
    }
    public static void AddCubeMesh(List<Vector3> Vectexes, List<Vector2> TextureUV, List<int> Triangles, float x, float y, float z, float Size, Vector2 TopUV, Vector2 BottomUV, Vector2 FrontUV, Vector2 RightUV, Vector2 BehindUV, Vector2 LeftUV, bool Left = true, bool Right = true, bool Bottom = true, bool Top = true, bool Front = true, bool Behind = true)
    {
        int VecCount = Vectexes.Count;
        double BlockWidth = 0.0625, BlockHeight = 0.029397, Offset = 0.0007;
        if (Front)
        {

            Vectexes.Add(new Vector3(x, y, z));//0
            Vectexes.Add(new Vector3(x, y + Size, z));//1
            Vectexes.Add(new Vector3(x + Size, y + Size, z));//2
            Vectexes.Add(new Vector3(x + Size, y, z));//3
                                                      //Front
            TextureUV.Add(new Vector2((float)(FrontUV.x * BlockWidth + Offset), (float)(1 - (FrontUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(FrontUV.x * BlockWidth + Offset), (float)(1 - FrontUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((FrontUV.x + 1) * BlockWidth - Offset), (float)(1 - FrontUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((FrontUV.x + 1) * BlockWidth - Offset), (float)(1 - (FrontUV.y + 1) * BlockHeight + Offset)));

            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 0);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 3);

            VecCount += 4;
        }

        if (Behind)
        {
            //Behind
            Vectexes.Add(new Vector3(x, y, z + Size));//4
            Vectexes.Add(new Vector3(x, y + Size, z + Size));//5
            Vectexes.Add(new Vector3(x + Size, y + Size, z + Size));//6
            Vectexes.Add(new Vector3(x + Size, y, z + Size));//7

            //Behind
            TextureUV.Add(new Vector2((float)(BehindUV.x * BlockWidth + Offset), (float)(1 - (BehindUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(BehindUV.x * BlockWidth + Offset), (float)(1 - BehindUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((BehindUV.x + 1) * BlockWidth - Offset), (float)(1 - BehindUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((BehindUV.x + 1) * BlockWidth - Offset), (float)(1 - (BehindUV.y + 1) * BlockHeight + Offset)));

            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 3);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount);

            VecCount += 4;
        }

        if (Top)
        {
            //Top
            Vectexes.Add(new Vector3(x, y + Size, z));//1
            Vectexes.Add(new Vector3(x, y + Size, z + Size));//5
            Vectexes.Add(new Vector3(x + Size, y + Size, z + Size));//6
            Vectexes.Add(new Vector3(x + Size, y + Size, z));//2

            //Top
            TextureUV.Add(new Vector2((float)((TopUV.x + 1) * BlockWidth - Offset), (float)(1 - TopUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((TopUV.x + 1) * BlockWidth - Offset), (float)(1 - (TopUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(TopUV.x * BlockWidth + Offset), (float)(1 - (TopUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(TopUV.x * BlockWidth + Offset), (float)(1 - TopUV.y * BlockHeight - Offset)));

            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 3);

            VecCount += 4;
        }

        if (Bottom)
        {
            //Bottom
            Vectexes.Add(new Vector3(x, y, z + Size));//4
            Vectexes.Add(new Vector3(x, y, z));//0
            Vectexes.Add(new Vector3(x + Size, y, z));//3
            Vectexes.Add(new Vector3(x + Size, y, z + Size));//7

            //Bottom
            TextureUV.Add(new Vector2((float)(BottomUV.x * BlockWidth + Offset), (float)(1 - (BottomUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(BottomUV.x * BlockWidth + Offset), (float)(1 - BottomUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((BottomUV.x + 1) * BlockWidth - Offset), (float)(1 - BottomUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((BottomUV.x + 1) * BlockWidth - Offset), (float)(1 - (BottomUV.y + 1) * BlockHeight + Offset)));

            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 3);

            VecCount += 4;
        }

        if (Left)
        {
            //Left
            Vectexes.Add(new Vector3(x, y, z + Size));//4
            Vectexes.Add(new Vector3(x, y + Size, z + Size));//5
            Vectexes.Add(new Vector3(x, y + Size, z));//1
            Vectexes.Add(new Vector3(x, y, z));//0

            //Left
            TextureUV.Add(new Vector2((float)(LeftUV.x * BlockWidth + Offset), (float)(1 - (LeftUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(LeftUV.x * BlockWidth + Offset), (float)(1 - LeftUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((LeftUV.x + 1) * BlockWidth - Offset), (float)(1 - LeftUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((LeftUV.x + 1) * BlockWidth - Offset), (float)(1 - (LeftUV.y + 1) * BlockHeight + Offset)));

            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 3);

            VecCount += 4;
        }

        if (Right)
        {
            //Right
            Vectexes.Add(new Vector3(x + Size, y, z));//3
            Vectexes.Add(new Vector3(x + Size, y + Size, z));//2
            Vectexes.Add(new Vector3(x + Size, y + Size, z + Size));//6
            Vectexes.Add(new Vector3(x + Size, y, z + Size));//7

            //Right
            TextureUV.Add(new Vector2((float)(RightUV.x * BlockWidth + Offset), (float)(1 - (RightUV.y + 1) * BlockHeight + Offset)));
            TextureUV.Add(new Vector2((float)(RightUV.x * BlockWidth + Offset), (float)(1 - RightUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((RightUV.x + 1) * BlockWidth - Offset), (float)(1 - RightUV.y * BlockHeight - Offset)));
            TextureUV.Add(new Vector2((float)((RightUV.x + 1) * BlockWidth - Offset), (float)(1 - (RightUV.y + 1) * BlockHeight + Offset)));

            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 1);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount);
            Triangles.Add(VecCount + 2);
            Triangles.Add(VecCount + 3);

            VecCount += 4;
        }
        //0.0625 0.029411
    }
}
