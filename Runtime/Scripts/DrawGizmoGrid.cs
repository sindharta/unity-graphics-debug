using UnityEngine;
using Shin.Core;

namespace Shin.GraphicsDebug {

[ExecuteInEditMode]
public class DrawGizmoGrid : CachedBehaviour {
    // rename + centre the gameobject upon first time dragging the script into the editor. 

#if UNITY_EDITOR
    void Reset () {
        if (name == "GameObject")
            name = "~~ GIZMO GRID ~~"; 
 
        GetTransform().position = Vector3.zero; 
    }
#endif

//----------------------------------------------------------------------------------------------------------------------

    void OnDrawGizmos () {

        Transform t = GetTransform();
        
        // orient to the gameobject, so you can rotate the grid independently if desired
        Gizmos.matrix = t.localToWorldMatrix;
 
        // set colours
        Color dimColor = new Color(m_gizmoLineColor.r, m_gizmoLineColor.g, m_gizmoLineColor.b, 0.25f* m_gizmoLineColor.a); 
        Color brightColor = Color.Lerp (Color.white, m_gizmoLineColor, 0.75f); 
 
        // draw the horizontal lines
        for (int x = m_minX; x < m_maxX+1; ++x) {
            Gizmos.color = ChooseColor(x, m_gizmoMajorLineCounter, m_gizmoLineColor, brightColor, dimColor);
 
            Vector3 pos1 = new Vector3(x, m_minY, 0) * m_gridScale;  
            Vector3 pos2 = new Vector3(x, m_maxY, 0) * m_gridScale;  
 
            // convert to topdown/overhead units if necessary
            if (m_topDownGrid) {
                pos1 = new Vector3(pos1.x, 0, pos1.y); 
                pos2 = new Vector3(pos2.x, 0, pos2.y); 
            }
 
            Gizmos.DrawLine ((m_gridOffset + pos1), (m_gridOffset + pos2)); 
        }
 
        // draw the vertical lines
        for (int y = m_minY; y < m_maxY+1; ++y) {
            Gizmos.color = ChooseColor(y, m_gizmoMajorLineCounter, m_gizmoLineColor, brightColor, dimColor);
 
            Vector3 pos1 = new Vector3(m_minX, y, 0) * m_gridScale;  
            Vector3 pos2 = new Vector3(m_maxX, y, 0) * m_gridScale;  
 
            // convert to topdown/overhead units if necessary
            if (m_topDownGrid) {
                pos1 = new Vector3(pos1.x, 0, pos1.y); 
                pos2 = new Vector3(pos2.x, 0, pos2.y); 
            }
 
            Gizmos.DrawLine ((m_gridOffset + pos1), (m_gridOffset + pos2)); 
        }
    }

//----------------------------------------------------------------------------------------------------------------------
    static Color ChooseColor(int index, int majorCounter, Color gizmoColor, Color brightColor, Color dimColor) {
        Color ret;
        if (index== 0)
            ret = brightColor;
        else {
            ret = (index % majorCounter== 0 ? gizmoColor: dimColor); 
        }

        return ret;

    }
//----------------------------------------------------------------------------------------------------------------------

    // universal grid scale
    [SerializeField] float m_gridScale = 1f; 
 
    // extents of the grid
    [SerializeField] int m_minX = -15; 
    [SerializeField] int m_minY = -15; 
    [SerializeField] int m_maxX = 15; 
    [SerializeField] int m_maxY = 15; 
 
    // nudges the whole grid rel
    [SerializeField] Vector3 m_gridOffset = Vector3.zero; 
 
    // is this an XY or an XZ grid?
    [SerializeField] bool m_topDownGrid = true; 
 
    // choose a colour for the gizmos
    [SerializeField] int m_gizmoMajorLineCounter = 5; 
    [SerializeField] Color m_gizmoLineColor = new Color (0.9f, 0.1f, 0.1f, 1f);  

}

} //end namespace

//Reference: https://wiki.unity3d.com/index.php/DrawGizmoGrid