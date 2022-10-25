using UnityEngine;

namespace Player
{
    public class CoinCounter : MonoBehaviour
    {
        private float _money;


        public void GetMoney(float moneyFromObjects)
        {
            _money += moneyFromObjects;
            MoneyConsoleWriter();
        }


        public void MoneyConsoleWriter()
        {
            Debug.Log($"Поздравляю! У Вас {_money} монет!");
        }
    }
}
