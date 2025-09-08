using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;

    private Unit selectedUnit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click to select
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Unit unit = hit.collider.GetComponentInParent<Unit>();
                if (unit != null)
                {
                    selectedUnit = unit;
                    Debug.Log($"Selected {unit.data.unitName}");
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) // right click to move
        {
            if (selectedUnit != null)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                selectedUnit.MoveTo(mousePos);
            }
        }
    }
}
