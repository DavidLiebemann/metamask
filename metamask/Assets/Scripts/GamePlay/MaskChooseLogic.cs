using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class MaskChooseLogic : MonoBehaviour
    {
        private GameObject[] _masks;
        private int _activeMaskId = 0;

        public string SelectedMaskName
        {
            get { return _masks[_activeMaskId].name; }
        }

        private void OnEnable()
        {
            _masks = new GameObject[transform.childCount];
            _activeMaskId = Random.Range(0, _masks.Length);
            for (int i = 0; i < transform.childCount; i++)
            {
                _masks[i] = transform.GetChild(i).gameObject;
            }

            UpdateMasksActive();
        }

        public void UpdateMasksActive()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _masks[i].SetActive(i == _activeMaskId);
            }
        }


        public void NextMask()
        {
            _activeMaskId = (_activeMaskId + 1) % _masks.Length;
            UpdateMasksActive();
        }

        public void PreviousMask()
        {
            _activeMaskId = (_activeMaskId - 1) % _masks.Length;
            if (_activeMaskId < 0)
            {
                _activeMaskId += _masks.Length;
            }

            UpdateMasksActive();
        }
    }
}