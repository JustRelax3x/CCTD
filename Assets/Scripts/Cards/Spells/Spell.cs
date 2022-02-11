using System.Collections;
namespace Assets.Scripts.Cards
{
    public class Spell : Card
    {
        public enum Type
        {
            Electrisity = 0,

            None = 100,
        }
        public Type SpellType;

        protected void CastHighestHp(int damage)
        {
            Enemy enemy = Game.GetHighestHpEnemy();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Game.VisualizeSpell(this, enemy.gameObject.transform);
            }
        }
        protected void CastRandom(int damage)
        {
            Enemy enemy = Game.GetRandomEnemy();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Game.VisualizeSpell(this, enemy.gameObject.transform);
            }
        }

        protected void Cast(IEnumerator enumerator)
        {
            Game.StartSpellCoroutine(enumerator);
        }
    }

}