using UnityEngine;

public class CreatureTouchReactionManager : MonoBehaviour
{
    // creatureのタップ、D&Dを処理して、Creatureのメソッドを呼ぶ
    Collider clickedCollider;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHitCreature(out clickedCollider);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Collider c;
            if (RaycastHitCreature(out c) && c == clickedCollider)
            {
                // creatureのステータス表示と、合成ボタン表示
                // creatureをフォーカス
                return;
            }
        }
    }

    bool RaycastHitCreature(out Collider c)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Creature"))
            {
                c = hit.collider;
                return true;
            }
        }
        c = null;
        return false;
    }

    public void DownMergeButton()
    {
        // MergeManager.SelectCreature(clickedCollider.gameObject.GetComponent<Creature>());
    }

}
