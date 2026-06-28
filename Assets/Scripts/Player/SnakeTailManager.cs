using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class SnakeTailManager : MonoBehaviour
    {
        [Header("Tail Settings")]
        [SerializeField] private GameObject bodyPrefab;
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private int initialBodySize = 3;

        [Tooltip("Jarak minimum untuk merekam posisi")]
        [SerializeField] private float minRecordDistance = 0.02f;

        [Tooltip("Delay antar segment")]
        [SerializeField] private int historyGap = 10;

        private List<Transform> bodyParts = new List<Transform>();

        private Transform tail;

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
            tailContainer =
                new GameObject("TailContainer_" + gameObject.name).transform;

            AllocateHistory(
                Mathf.Max(100, (initialBodySize + 3) * historyGap)
            );

            RecordMarker();

            SpawnInitialBody();

            CreateTail();
        }

        private void Update()
        {
            RecordMovement();
            UpdateBodyParts();
            UpdateTail();
        }

        private void RecordMovement()
        {
            bool record = true;

            if (historyCount > 0)
            {
                float sqr =
                    (transform.position -
                    GetMarker(0).position).sqrMagnitude;

                if (sqr < minRecordDistance * minRecordDistance)
                    record = false;
            }

            if (record)
                RecordMarker();
        }

        private void CreateTail()
        {
            if (tail != null)
                return;

            Vector3 spawnPos =
                GetSpawnBehindHead(bodyParts.Count);

            GameObject obj =
                Instantiate(
                    tailPrefab,
                    spawnPos,
                    transform.rotation,
                    tailContainer
                );

            tail = obj.transform;
        }

        private void UpdateTail()
        {
            if (tail == null)
                return;

            int offset =
                (bodyParts.Count + 1) * historyGap;

            if (offset < historyCount)
            {
                Marker marker = GetMarker(offset);

                tail.position = marker.position;
                tail.rotation = marker.rotation;
            }
        }

        private void UpdateBodyParts()
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                int offset =
                    (i + 1) * historyGap;

                if (offset < historyCount)
                {
                    Marker marker =
                        GetMarker(offset);

                    bodyParts[i].position =
                        marker.position;

                    bodyParts[i].rotation =
                        marker.rotation;
                }
            }
        }

        public void Grow(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Marker marker =
                    GetMarker(
                        (bodyParts.Count + 1)
                        * historyGap
                    );

                GameObject body =
                    Instantiate(
                        bodyPrefab,
                        marker.position,
                        marker.rotation,
                        tailContainer
                    );

                bodyParts.Add(body.transform);
            }
        }

        private void AllocateHistory(int size)
        {
            positionHistory =
                new Marker[size];

            historyIndex = 0;
        }

        private void RecordMarker()
        {
            int required =
                (bodyParts.Count + 3)
                * historyGap;

            if (positionHistory.Length < required)
            {
                AllocateHistory(required * 2);
            }

            historyIndex--;

            if (historyIndex < 0)
                historyIndex =
                    positionHistory.Length - 1;

            positionHistory[historyIndex] =
                new Marker(
                    transform.position,
                    transform.rotation
                );

            if (historyCount < positionHistory.Length)
                historyCount++;
        }

        private Marker GetMarker(int offset)
        {
            if (offset >= historyCount)
                offset = historyCount - 1;

            int index =
                (historyIndex + offset)
                % positionHistory.Length;

            return positionHistory[index];
        }

        private Vector3 GetSpawnBehindHead(int index)
        {
            Vector3 backward =
                -transform.up * (index + 1);

            return transform.position + backward;
        }
        private void SpawnInitialBody()
        {
            for (int i = 0; i < initialBodySize; i++)
            {
                Vector3 spawnPos =
                    GetSpawnBehindHead(i);


                GameObject body =
                    Instantiate(
                        bodyPrefab,
                        spawnPos,
                        transform.rotation,
                        tailContainer
                    );
                bodyParts.Add(body.transform);
            }
        }
    }
}