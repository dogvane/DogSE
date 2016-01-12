using UnityEngine;

namespace Assets.Scripts.Ship.UIUtils
{
    [AddComponentMenu("Exploration/Ship Sail")]
    public class ShipSail : MonoBehaviour
    {
        // Renderer associated with the sails
        public Renderer sailRenderer;

        // Due to the alpha channel of the texture used for this shader, the sail
        // damage becomes visible after 0.2, and becomes too much after 0.55.
        public Vector2 cutoffRange = new Vector2(0.2f, 0.55f);

        //private GameShip mStats;

        /// <summary>
        /// Cache the stats.
        /// </summary>

        private void Start()
        {
            //mStats = GameShip.Find(transform);
        }

        /// <summary>
        /// Damage the sail when hit by a cannonball.
        /// </summary>

        private void OnTriggerEnter(Collider col)
        {
            //Cannonball cb = col.GetComponent<Cannonball>();

            //if (cb != null && cb.damage > 0f)
            //{
            //    // Damage the sails
            //    float damage = mStats.ApplyDamageToSails(cb.damage);

            //    // Print the damage text over the sail
            //    if (damage > 0f)
            //        ScrollingCombatText.Print(gameObject, "-" + Mathf.RoundToInt(damage), new Color(1f, .4f, 0f));
            //}
        }

        /// <summary>
        /// Keep the sail material updated with the current sail health.
        /// </summary>

        private void Update()
        {
            //if (sailRenderer != null && mStats != null)
            //{
            //    float health = mStats.sailHealth.x/mStats.sailHealth.y;
            //    float cutoff = Mathf.Lerp(cutoffRange.y, cutoffRange.x, health);
            //    sailRenderer.material.SetFloat("_Cutoff", cutoff);
            //}
        }
    }
}