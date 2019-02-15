using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class Chuck : MonoBehaviour {
    public Map map;
    public Chuck LeftChuck, RightChuck, BehindChuck, FrontChuck;
    public Vector2 Index;
    public bool HasPresent, HasLoad;
    public List<Vector3> Vectexes = new List<Vector3>();
    public List<Vector2> TextureUV = new List<Vector2>();
    public List<int> Triangles = new List<int>();
    public Block[,,] Blocks = new Block[16, 255, 16];

    public void Load(Map Parent,Vector2 ChuckIndex,Material material)
    {
        map = Parent;
        Index = ChuckIndex;
        GetComponent<MeshRenderer>().material = material;
        Create();

    }
    public void Create()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                int h =80 +  (int)(Mathf.PerlinNoise(0.05f * (x + Index.x * 16), 0.05f * (z + Index.y * 16)) * 30);

                for (int y = 0; y < h; y++)
                {
                    Blocks[x, y, z] = new Block(this, BlockInfo.BlockID.Dirt,false,new Vector3(x, y, z)); 
                }
                Blocks[x, h, z] = new Block(this, BlockInfo.BlockID.Dirt, false, new Vector3(x, h, z));
            }
        }
        HasLoad = true;
    }

    public void OnMouseOver()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)),out hit,5);
        GameObject cube = GameObject.Find("SelectCube");
        cube.transform.position = new Vector3((int)hit.point.x+0.5f, (int)(hit.point.y-0.1f)+0.5f, (int)hit.point.z+0.5f);
    }
    public void OnMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, 5);
        GameObject cube = GameObject.Find("SelectCube");
        if (Input.GetMouseButton(0))
        {
            if (map.Player.transform.position.y + 0.5f > hit.point.y)
                map[(int)hit.point.x, (int)(hit.point.y - 0.5f), (int)hit.point.z].ID = BlockInfo.BlockID.Empty;
            else
                map[(int)hit.point.x, (int)(hit.point.y), (int)hit.point.z].ID = BlockInfo.BlockID.Empty;
        }
        else if (Input.GetMouseButton(1))
            map[(int)hit.point.x, (int)(hit.point.y), (int)hit.point.z] = new Block(this, BlockInfo.BlockID.Dirt, false, new Vector3((int)hit.point.x, (int)(hit.point.y), (int)hit.point.z));
    }

    IEnumerator PresentBlock()
    {
        Vectexes.Clear();
        Triangles.Clear();
        TextureUV.Clear();
        //if (!HasLoad) yield return 0;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 255; y++)
                {
                    if (Blocks[x, y, z] != null)
                    {
                        Blocks[x, y, z].Present();
                    }
                }
                yield return 0;

            }


        }
        mesh.vertices = Vectexes.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.uv = TextureUV.ToArray();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Present()
    {
        HasPresent = true;
        StartCoroutine(PresentBlock());
    }
    public void PresentImmediately()
    {
        HasPresent = true;
        Vectexes.Clear();
        Triangles.Clear();
        TextureUV.Clear();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 255; y++)
                {
                    if (Blocks[x, y, z] != null)
                    {
                        Blocks[x, y, z].Present();
                    }
                }

            }
        }
        mesh.vertices = Vectexes.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.uv = TextureUV.ToArray();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    public void FixedUpdate()
    {
        if (Vector3.Distance(map.Player.transform.position, transform.position) > 15*16) Destroy(gameObject);
    }
    public void OnDestroy()
    {
        map.ChuckStream.Remove(Index);
    }
}
