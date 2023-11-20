
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class HierarchyStyle : MonoBehaviour
{

    public class Style
    {
        public delegate void HierarchyStyleButtonDelegate();

        public Texture2D icon;
        public Color backgroundColor;
        public bool bold = false;
        public UnityEvent OnButtonPush;
        public string buttonTitle;

        public Style(Texture2D icon, Color backgroundColor, bool bold, UnityEvent onButtonPush, string buttonTitle)
        {
            this.icon = icon;
            this.backgroundColor = backgroundColor;
            this.bold = bold;
            this.OnButtonPush = onButtonPush;
            this.buttonTitle = buttonTitle;
        }
    }

    public static Dictionary<GameObject, Style> AttachedObjects = new Dictionary<GameObject, Style>();

    [SerializeField] private bool bold = false;
    [SerializeField] private Texture2D icon;
    [SerializeField] private Color backgroundColor;

    [Header("Button")]
    [SerializeField] private string buttonTitle = "-X-";
    public UnityEvent OnButtonPush;

    private void Awake()
    {
#if UNITY_EDITOR
        if (!AttachedObjects.ContainsKey(gameObject))
            AttachedObjects.Add(gameObject, new Style(icon, backgroundColor, bold, OnButtonPush, buttonTitle));

        ValidateList();
#endif
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!AttachedObjects.ContainsKey(gameObject))
            AttachedObjects.Add(gameObject, new Style(icon, backgroundColor, bold, OnButtonPush, buttonTitle));
        else
            AttachedObjects[gameObject] = new Style(icon, backgroundColor, bold, OnButtonPush, buttonTitle);

        EditorApplication.RepaintHierarchyWindow();
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        AttachedObjects.Remove(gameObject);
        ValidateList();
#endif
    }
    
    //[Button]
    private void ValidateList()
    {
#if UNITY_EDITOR
        List<GameObject> nullObjets = new List<GameObject>();
        foreach(GameObject go in AttachedObjects.Keys)
        {
            if (go == null)
                nullObjets.Add(go);
        }

        foreach (GameObject go in nullObjets)
        {
            AttachedObjects.Remove(go);
        }
#endif
    }

}

#if UNITY_EDITOR
[InitializeOnLoad]
public class DrawHierarcyStyle
{

    private static GameObject attachedGameObject;
    private static Rect iconRect;
    private static Rect buttonRect;
    private static Texture2D initialBackgroundTexture;
    private static Texture2D backgroundTexture;
    private static Rect backgroundRect;
    private static Rect nameRect;
    private static GUIContent nameContent;
    private static Color defaultBackgroundColor;

    static DrawHierarcyStyle()
    {
        backgroundTexture = new Texture2D(1, 1);
        nameContent = new GUIContent();
        defaultBackgroundColor = new Color(0.21960f, 0.21960f, 0.21960f);
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        EditorApplication.RepaintHierarchyWindow();
    }

    private static void HierarchyItemCB(int instanceId, Rect selectionRect)
    {
        attachedGameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

        if (attachedGameObject == null)
            return;

        if(backgroundTexture == null)
            backgroundTexture = new Texture2D(1, 1);

        if (nameContent == null)
            nameContent = new GUIContent();

        if (HierarchyStyle.AttachedObjects.ContainsKey(attachedGameObject) && HierarchyStyle.AttachedObjects[attachedGameObject] != null)
        {
            backgroundRect = selectionRect;
            backgroundRect.x += 16f;
            backgroundRect.width -= 16f;

            //set default background color
            if (HierarchyStyle.AttachedObjects[attachedGameObject].backgroundColor.a > 0.01f)
            {
                backgroundTexture.SetPixel(0, 0, defaultBackgroundColor);
                backgroundTexture.Apply();
                initialBackgroundTexture = GUI.skin.box.normal.background;
                GUI.skin.box.normal.background = backgroundTexture;
                GUI.Box(backgroundRect, GUIContent.none);
                GUI.skin.box.normal.background = initialBackgroundTexture;
            }

            //set background color
            backgroundTexture.SetPixel(0, 0, HierarchyStyle.AttachedObjects[attachedGameObject].backgroundColor);
            backgroundTexture.Apply();
            initialBackgroundTexture = GUI.skin.box.normal.background;
            GUI.skin.box.normal.background = backgroundTexture;
            GUI.Box(backgroundRect, GUIContent.none);
            GUI.skin.box.normal.background = initialBackgroundTexture;

            //set icon
            if (HierarchyStyle.AttachedObjects[attachedGameObject].icon != null)
            {
                iconRect = selectionRect;
                iconRect.x = iconRect.width + (selectionRect.x - 16f);
                iconRect.width = 16f;
                iconRect.height = 16f;
                GUI.DrawTexture(iconRect, HierarchyStyle.AttachedObjects[attachedGameObject].icon);
            }

            //set button
            if (HierarchyStyle.AttachedObjects[attachedGameObject].OnButtonPush != null && HierarchyStyle.AttachedObjects[attachedGameObject].OnButtonPush.GetPersistentEventCount() > 0)
            {
                buttonRect = selectionRect;
                buttonRect.x = selectionRect.x + selectionRect.width - 35f - (HierarchyStyle.AttachedObjects[attachedGameObject].buttonTitle.Length * 7f);
                buttonRect.width = 15f + (HierarchyStyle.AttachedObjects[attachedGameObject].buttonTitle.Length * 7f);
                buttonRect.height = 16f;
                if (GUI.Button(buttonRect, HierarchyStyle.AttachedObjects[attachedGameObject].buttonTitle))
                    HierarchyStyle.AttachedObjects[attachedGameObject].OnButtonPush.Invoke();
            }

            //set bold
            nameRect = selectionRect;
            nameRect.y -= 1f;
            nameRect.x += 16f;
            nameContent.text = attachedGameObject.name;
            if(HierarchyStyle.AttachedObjects[attachedGameObject].bold)
                GUI.Label(nameRect, nameContent, EditorStyles.boldLabel);
            else if(HierarchyStyle.AttachedObjects[attachedGameObject].backgroundColor.a > 0.01f)
                GUI.Label(nameRect, nameContent);
        }
    }
}

#endif