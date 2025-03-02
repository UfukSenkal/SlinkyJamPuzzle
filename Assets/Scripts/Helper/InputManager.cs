using HybridPuzzle.SlinkyJam.Slinky;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Helper
{

    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    SlinkySegment segment = hit.collider.GetComponent<SlinkySegment>();
                    if (segment != null)
                    {
                        segment.OnSegmentClicked();
                    }
                }
            }
        }
    }
}