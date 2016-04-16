using UnityEngine;
using System.Collections;

public class gridOverlay : MonoBehaviour
{

    public bool showMain = true;
    public bool showSub = false;

    public int gridSizeX = 200;
    public int gridSizeY = 0 ;
    public int gridSizeZ = 200;

    [Range(1, 1000)]
    public float smallStep = 10;
    [Range(1, 1000)]
    public float largeStep = 100;

    public float startX = 0;
    public float startY = 0;
    public float startZ = 0;

    private float offsetY = 0;
    private float scrollRate = 0.1f;
    private float lastScroll = 0f;

    private Material lineMaterial;

    public Color mainColor = new Color(0f, 1f, 0f, 0.1f);
    public Color subColor = new Color(0f, 0.5f, 0f, 0.1f);

    public float lineWidth = 3;

    void Start()
    {
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();

        lineMaterial = CreateLineMaterial(lineMaterial);
    }

    void Update()
    {
        
        if (lastScroll + scrollRate < Time.time)
        {
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
//                plane.transform.position += smallStep * Vector3.up;                
                offsetY += smallStep;
                lastScroll = Time.time;
            }
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
//                plane.transform.position -= smallStep * Vector3.up;
                offsetY -= smallStep;
                lastScroll = Time.time;
            }
        }
    }
    static public Material CreateLineMaterial(Material lineMaterial)
    {        
        if (lineMaterial == null)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);            
        }
        return lineMaterial;
    }

    // to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
    static public void DrawQuad(Vector3 p1, Vector3 p2, float lineWidth) {
        float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
        Vector3 edge1 = Camera.main.transform.position - (p2 + p1) / 2.0f;    //vector from line center to camera
        Vector3 edge2 = p2 - p1;    //vector from point to point
        Vector3 perpendicular = Vector3.Cross(edge1, edge2).normalized * thisWidth;

        GL.Vertex(p1 - perpendicular);
        GL.Vertex(p1 + perpendicular);
        GL.Vertex(p2 + perpendicular);
        GL.Vertex(p2 - perpendicular);
    }
    static public void DrawLine(Vector3 v1, Vector3 v2){
        GL.Vertex(v1);
        GL.Vertex(v2);
    }
    void OnDrawLine(Vector3 v1, Vector3 v2) {
        if (lineWidth == 1)
            DrawLine(v1, v2);
        else
            DrawQuad(v1, v2, lineWidth);
    }
    void OnPostRender()
    {
        
        // set the current material
        lineMaterial.SetPass(0);

        if (lineWidth == 1)
            GL.Begin(GL.LINES);
        else
            GL.Begin(GL.QUADS);

        if (showSub)
        {
            GL.Color(subColor);

            //Layers
            for (float j = 0; j <= gridSizeY; j += smallStep)
            {
                //X axis lines
                for (float i = 0; i <= gridSizeZ; i += smallStep)
                {
                    OnDrawLine(
                        new Vector3(startX, j + offsetY, startZ + i), 
                        new Vector3(gridSizeX, j + offsetY, startZ + i));
                }

                //Z axis lines
                for (float i = 0; i <= gridSizeX; i += smallStep)
                {
                    OnDrawLine(
                        new Vector3(startX + i, j + offsetY, startZ),
                        new Vector3(startX + i, j + offsetY, gridSizeZ) );
                }
            }

            //Y axis lines
            for (float i = 0; i <= gridSizeZ; i += smallStep)
            {
                for (float k = 0; k <= gridSizeX; k += smallStep)
                {
                    OnDrawLine(
                        new Vector3(startX + k, startY + offsetY, startZ + i),
                        new Vector3(startX + k, gridSizeY + offsetY, startZ + i));
                }
            }
        }

        if (showMain)
        {
            GL.Color(mainColor);

            //Layers
            for (float j = 0; j <= gridSizeY; j += largeStep)
            {
                //X axis lines
                for (float i = 0; i <= gridSizeZ; i += largeStep)
                {
                    OnDrawLine(
                        new Vector3(startX, j + offsetY, startZ + i),
                        new Vector3(gridSizeX, j + offsetY, startZ + i));
                }

                //Z axis lines
                for (float i = 0; i <= gridSizeX; i += largeStep)
                {
                    OnDrawLine(
                        new Vector3(startX + i, j + offsetY, startZ),
                        new Vector3(startX + i, j + offsetY, gridSizeZ));
                }
            }

            //Y axis lines
            for (float i = 0; i <= gridSizeZ; i += largeStep)
            {
                for (float k = 0; k <= gridSizeX; k += largeStep)
                {
                    OnDrawLine(
                        new Vector3(startX + k, startY + offsetY, startZ + i),
                        new Vector3(startX + k, gridSizeY + offsetY, startZ + i));
                }
            }
        }


        GL.End();
    }
}