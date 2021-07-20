using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions;
#endif
 
/// <summary>
/// 标题属性
/// <para>ZhangYu 2018-06-21</para>
/// </summary>
#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Field)]
#endif
public class TitleAttribute : PropertyAttribute {
 
    /// <summary> 标题名称 </summary>
    public string title;
    /// <summary> 标题颜色 </summary>
    public string htmlColor;
 
    /// <summary> 在属性上方添加一个标题 </summary>
    /// <param name="title">标题名称</param>
    /// <param name="htmlColor">标题颜色</param>
    public TitleAttribute(string title, string htmlColor = "#FFFFFF") {
        this.title = title;
        this.htmlColor = htmlColor;
    }
 
}
 
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TitleAttribute))]
public class TitleAttributeDrawer : DecoratorDrawer {
 
    // 文本样式
    private GUIStyle style = new GUIStyle();
 
    public override void OnGUI(Rect position) {
        // 获取Attribute
        TitleAttribute attr = (TitleAttribute)attribute;
 
        // 转换颜色
        Color color = htmlToColor(attr.htmlColor);
 
        // 重绘GUI
        position = EditorGUI.IndentedRect(position);
        style.normal.textColor = color;
        GUI.Label(position, attr.title, style);
    }
 
    public override float GetHeight() {
        return base.GetHeight() - 4;
    }
 
    /// <summary> Html颜色转换为Color </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private Color htmlToColor(string hex) {
        // 默认黑色
        if (string.IsNullOrEmpty(hex)) return Color.black;
 
        // 转换颜色
        hex = hex.ToLower();
        if (hex.IndexOf("#") == 0 && hex.Length == 7) {
            int r = Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return new Color(r / 255f, g / 255f, b / 255f);
        } else if (hex == "red") {
            return Color.red;
        } else if (hex == "green") {
            return Color.green;
        } else if (hex == "blue") {
            return Color.blue;
        } else if (hex == "yellow") {
            return Color.yellow;
        } else if (hex == "black") {
            return Color.black;
        } else if (hex == "white") {
            return Color.white;
        } else if (hex == "cyan") {
            return Color.cyan;
        } else if (hex == "gray") {
            return Color.gray;
        } else if (hex == "grey") {
            return Color.grey;
        } else if (hex == "magenta") {
            return Color.magenta;
        } else {
            return Color.black;
        }
    }
 
}
#endif
 
/// <summary>
/// 设置枚举名称
/// <para>ZhangYu 2018-06-21</para>
/// </summary>
 
#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Field)]
#endif
public class EnumNameAttribute : PropertyAttribute {
 
    /// <summary> 枚举名称 </summary>
    public string name;
 
    public EnumNameAttribute(string name) {
        this.name = name;
    }
 
}
 
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumNameAttribute))]
public class EnumNameDrawer : PropertyDrawer {
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // 替换属性名称
        EnumNameAttribute rename = (EnumNameAttribute)attribute;
        label.text = rename.name;
 
        // 重绘GUI
        bool isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement) label.text = property.displayName;
        if (property.propertyType == SerializedPropertyType.Enum) {
            drawEnum(position, property, label);
        } else {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
 
    // 绘制枚举类型
    private void drawEnum(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginChangeCheck();
 
        // 获取枚举相关属性
        Type type = fieldInfo.FieldType;
        string[] names = property.enumNames;
        string[] values = new string[names.Length];
        while (type.IsArray) type = type.GetElementType();

 
        // 获取枚举所对应的名称
        for (int i = 0; i < names.Length; i++) {
            FieldInfo info = type.GetField(names[i]);
            EnumNameAttribute[] atts = (EnumNameAttribute[])info.GetCustomAttributes(typeof(EnumNameAttribute), false);
            values[i] = atts.Length == 0 ? names[i] : atts[0].name;

            Debug.Log(names[i]);
        }
 
        // 重绘GUI
        int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1) property.enumValueIndex = index;
    }
 
}
#endif