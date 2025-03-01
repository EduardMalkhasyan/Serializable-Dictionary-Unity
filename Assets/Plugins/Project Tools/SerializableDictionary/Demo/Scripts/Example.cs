using ProjectTools.InterfaceHelp;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTools.DictionaryHelp

{
    public interface MyTestInterface
    {
        void TestMethod();
    }

    public class Example : MonoBehaviour, MyTestInterface
    {
        [SerializeField] private Text GeorgeLabel;
        [SerializeField] private Image GeorgeImage;

        [SerializeField] private Text ThemisLabel;
        [SerializeField] private Image ThemisImage;

        [SerializeField] private SerializableDictionary<int, string> intToStringDictionary;
        [SerializeField] private SerializableDictionary<string, Color> stringToColorDictionary;
        [SerializeField] private SerializableDictionary<string, int[]> stringToIntArrayDictionary;

        [SerializeField] private InterfaceHolder<MyTestInterface> serializedInterface;

        [SerializeField] private SerializableDictionary<string, InterfaceHolder<MyTestInterface>> stringToInterfaceDictionary;

        private const string GeorgeName = "George";
        private const string ThemisName = "Themis";

        private void Start()
        {
            TestSetup();
            serializedInterface.Value.TestMethod();
        }

        public void TestSetup()
        {
            GeorgeLabel.text = intToStringDictionary[0];
            ThemisLabel.text = intToStringDictionary[1];

            GeorgeImage.color = stringToColorDictionary[GeorgeName];
            ThemisImage.color = stringToColorDictionary[ThemisName];
        }

        [ContextMenu("Call")]
        public void CallTestMethodOfInterface()
        {
            stringToInterfaceDictionary["interface_1"].Value.TestMethod();
        }

        public void TestMethod()
        {
            Debug.Log($"Test method call from {this}");
        }
    }
}
