    using System.Collections;
    using UnityEngine;

    public class ObjectScript : MonoBehaviour
    {
        [SerializeField] private ItemsData _data;
        [SerializeField] private float maxScaleTime;

        private Rigidbody2D rigidb;
        private Coroutine scaleRoutine;
        private float currentScaleMultiplier;

        private void Start()
        {
            rigidb = GetComponent<Rigidbody2D>();
            rigidb.mass = _data.itemMass;
        }

        public void ThrowThis(Vector2 throwDirection,float maxThrowForce, float forceMultiplier)
        {
            if(scaleRoutine != null)
                StopCoroutine(scaleRoutine);

            transform.SetParent(null);
            rigidb.simulated = true;

            rigidb.AddForce(throwDirection * maxThrowForce * forceMultiplier, ForceMode2D.Impulse);
            scaleRoutine = StartCoroutine(ScaleCoroutine());
        }

        private IEnumerator ScaleCoroutine()
        {
            currentScaleMultiplier = 0f;
        
            while(currentScaleMultiplier < maxScaleTime)
            {
           
                currentScaleMultiplier += Time.deltaTime;   
                float normalize = Mathf.Clamp01(currentScaleMultiplier / maxScaleTime);
                /*Reads the animation curve setted on a database, so the item changes scale, providing an illusion of the item going up, on the Z axis
                    and puts that values into a variable*/
                float scale = _data.throwCurve.Evaluate(normalize);

                transform.localScale = Vector3.one * scale;
                transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.y, 0.3f, 0.45f);

                yield return null;
            }
            rigidb.linearVelocity = Vector2.zero;
            scaleRoutine = null;
        }

        public float GetItemWheight() { return _data.itemMass; }
    }