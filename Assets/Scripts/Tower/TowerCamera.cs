using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Tower
{
    public class TowerCamera : MonoBehaviour
    {
        private int _focusedFloor;
        public float animationTime = .75f;
        private float floorHeight => Tower.Instance.levelHeight;

        private void Start()
        {
            transform.position = new Vector3(0, floorHeight / 2);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Move(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                Move(-1);
        }

        private void Move(int dir)
        {
            Show(_focusedFloor + dir);
        }

        public void Show(int targetFloor)
        {
            if (targetFloor < 0 || targetFloor >= Tower.Instance.level || targetFloor == _focusedFloor) return;
            LeanTween.moveY(gameObject, floorHeight * targetFloor + floorHeight / 2, animationTime).setEaseOutCirc().setOnComplete(() =>
            {
                _focusedFloor = targetFloor;
                // transform.position = new Vector3(0, floorHeight * targetFloor + floorHeight / 2);
            });
        }
    }
}
