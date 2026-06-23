using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class SnakeTailManager : MonoBehaviour
    {
        [Header("Tail Settings")]
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private int initialTailSize = 0;

        [Tooltip("Delay antar segment (lebih kecil = lebih rapat)")]
        [SerializeField] private int historyGap = 10;

        private List<GameObject> bodyParts = new List<GameObject>();
        private Transform tailContainer;

        private Marker[] positionHistory;
        private int historyIndex = 0;
        private int historyCount = 0;

        private struct Marker
        {
            public Vector3 position;
            public Quaternion rotation;

            public Marker(Vector3 pos, Quaternion rot)
            {
                position = pos;
                rotation = rot;
            }
        }

        private void Start()
        {
            tailContainer = new GameObject("TailContainer_" + gameObject.name).transform;

            AllocateHistory(Mathf.Max(100, (initialTailSize + 2) * historyGap));
            RecordMarker();

            for (int i = 0; i < initialTailSize; i++)
            {
                Grow();
            }
        }

        private void OnDestroy()
        {
            if (tailContainer != null)
                Destroy(tailContainer.gameObject);
        }

        private void Update()
        {
            RecordMarker();
            UpdateBodyParts();
        }

        private void AllocateHistory(int newSize)
        {
            Marker[] newHistory = new Marker[newSize];

            if (positionHistory != null && historyCount > 0)
            {
                for (int i = 0; i < historyCount; i++)
                {
                    newHistory[i] = GetMarker(i);
                }
            }

            positionHistory = newHistory;
            historyIndex = 0;
        }

        private void RecordMarker()
        {
            int requiredSize = (bodyParts.Count + 1) * historyGap + 1;

            if (positionHistory == null || positionHistory.Length < requiredSize)
            {
                AllocateHistory(requiredSize * 2);
            }

            historyIndex--;
            if (historyIndex < 0)
                historyIndex = positionHistory.Length - 1;

            positionHistory[historyIndex] =
                new Marker(transform.position, transform.rotation);

            if (historyCount < positionHistory.Length)
                historyCount++;
        }

        private Marker GetMarker(int offset)
        {
            if (offset >= historyCount)
                offset = historyCount - 1;

            int actualIndex = (historyIndex + offset) % positionHistory.Length;
            return positionHistory[actualIndex];
        }

        private void UpdateBodyParts()
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                int pointOffset = (i + 1) * historyGap;

                if (pointOffset < historyCount)
                {
                    Marker target = GetMarker(pointOffset);

                    bodyParts[i].transform.position = target.position;
                    bodyParts[i].transform.rotation = target.rotation;
                }
            }
        }

        public void Grow(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPos = transform.position;
                Quaternion spawnRot = transform.rotation;

                int endOffset = (bodyParts.Count + 1) * historyGap;

                if (historyCount > 0)
                {
                    Marker endMarker = GetMarker(endOffset);
                    spawnPos = endMarker.position;
                    spawnRot = endMarker.rotation;
                }

                GameObject newPart =
                    Instantiate(tailPrefab, spawnPos, spawnRot, tailContainer);

                bodyParts.Add(newPart);
            }
        }
    }
}