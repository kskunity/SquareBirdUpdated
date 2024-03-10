using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    public List<Transform> m_transforms;
    [Space]
    public List<Vector3> m_positions;
    [Space]
    public List<float> m_times;
    [Space]
    public _Effects m_effect;
    [Space]
    public Vector3 m_start_pos;
    public Vector3 m_last_pos;
    [Space]
    public float m_time;
    [Space]
    public Ease m_ease;
    [Space]
    public bool m_initial;
    private Sequence m_seq;

    private float m_ypos;

    private bool m_used;

    int m_total_transform_to_move;
    int m_initial_index;


    void Start()
    {
        if (!m_initial) return;
        _DoAnimation();
    }

    public void _DoAnimation()
    {
        m_used = true;
        switch (m_effect)
        {
            case _Effects.m_a:
                m_seq = DOTween.Sequence();
                m_seq.Append(transform.DOLocalMoveY(m_last_pos.y, m_time)).SetEase(m_ease);
                //m_seq.Append(transform.DOLocalMoveY(m_start_pos.y, m_time)).SetEase(m_ease);
                m_seq.SetLoops(1);
                break;
            case _Effects.m_b:
                m_seq = DOTween.Sequence();
                m_seq.Append(m_transforms[0].DOLocalMoveY(m_positions[0].y, m_time)).SetEase(m_ease);
                m_seq.Append(m_transforms[1].DOLocalMoveY(m_positions[1].y, m_time)).SetEase(m_ease);
                m_seq.Append(m_transforms[2].DOLocalMoveY(m_positions[2].y, m_time)).SetEase(m_ease);
                m_seq.SetLoops(1);
                break;
            case _Effects.m_c:
                m_seq = DOTween.Sequence();
                m_seq.Append(m_transforms[0].DOLocalMoveY(m_last_pos.y, m_time)).SetEase(m_ease);
                m_seq.SetLoops(1);
                break;
                //RANDOM Y POS
            case _Effects.m_d:
                m_ypos = m_positions[Random.Range(0, m_positions.Count)].y;
                m_seq = DOTween.Sequence();
                m_seq.Append(m_transforms[0].DOLocalMoveY(m_ypos, m_time)).SetEase(m_ease);
                m_seq.SetLoops(1);
                break;
                //MOVE IN X
            case _Effects.m_e:
                m_seq = DOTween.Sequence();
                m_seq.Append(m_transforms[0].DOLocalMoveX(m_positions[0].x, m_times[0])).SetEase(m_ease);
                m_seq.Append(m_transforms[1].DOLocalMoveX(m_positions[1].x, m_times[1])).SetEase(m_ease);
                m_seq.Append(m_transforms[2].DOLocalMoveX(m_positions[2].x, m_times[2])).SetEase(m_ease);
                m_seq.SetLoops(1);
                break;

                //MOVE X POSITION IN ALL IN LIST
            case _Effects.m_f:
                m_total_transform_to_move = m_transforms.Count - 1;
                m_initial_index = 0;
                m_seq = DOTween.Sequence();
                foreach (var item in m_transforms)
                {
                    m_seq.Append(m_transforms[m_initial_index].DOLocalMoveX(m_positions[m_initial_index].x, m_times[m_initial_index])).SetEase(m_ease);
                    m_initial_index++;
                }
                m_seq.SetLoops(1);

                break;
            case _Effects.m_g:
                break;
            case _Effects.m_h:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_used) return;

        switch (other.tag)
        {
            case "Bird":
                _DoAnimation();
                break;
        }
    }

}

[System.Serializable]
public enum _Effects
{
    m_none,
    m_a,
    m_b,
    m_c,
    m_d,
    m_e,
    m_f,
    m_g,
    m_h,
    m_i,
    m_j,
    m_k,
    m_l
}
