using Codice.CM.Client.Gui;
using UnityEditor;
using UnityEngine;

public class BehaviorTreeEditorWindow : EditorWindow
{
    private BehaviorTree _behaviorTree;
    private Vector2 _scrollPosition;

    [MenuItem("MeowTools/Behavior Tree Editor")]
    public static void OpenWindow()
    {
        GetWindow<BehaviorTreeEditorWindow>("Behavior Tree Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Behavior Tree Editor", EditorStyles.boldLabel);

        _behaviorTree = (BehaviorTree)EditorGUILayout.ObjectField("Behavior Tree", _behaviorTree, typeof(BehaviorTree), false);

        if (_behaviorTree == null)
        {
            GUILayout.Label("Select a Behavior Tree to edit.");
            return;
        }
        Draw();
        HandleEvents(Event.current);

        if (GUI.changed) Repaint();
    }
    private void HandleEvents(Event e)
    {
        //TODO: ADD NODE MENU
    }
    private void Draw()
    {
        GUILayout.Space(20);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));
        DrawRootNode(_behaviorTree.GetRootNode());
        GUILayout.EndScrollView();
    }
    private void DrawRootNode(RootNode rootNode)
    {
        GUIStyle textStyle = new(EditorStyles.boldLabel)
        {
            fontSize = 16
        };
        GUIStyle boxStyle = new()
        {
            padding = new RectOffset(10, 10, 10, 10),
            normal = { background = Texture2D.grayTexture },
            fontStyle = FontStyle.Bold
        };
        GUILayout.BeginArea(new Rect(position.width / 2 - 150, 20, 300, 80), boxStyle);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(_behaviorTree.treeName, textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(rootNode.nodeName, textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndArea();
        if (rootNode.childNode != null)
        {
            DrawNode(rootNode.childNode, new Vector2(position.width / 2 , position.height / 2 + 10));
        }
    }

    private void DrawSelectorNode(SelectorNode selectorNode, Vector2 position)
    {
        GUIStyle textStyle = new(EditorStyles.boldLabel)
        {
            fontSize = 16
        };
        GUIStyle boxStyle = new()
        {
            padding = new RectOffset(10, 10, 10, 10),
            normal = { background = Texture2D.grayTexture },
            fontStyle = FontStyle.Bold
        };
        GUILayout.BeginArea(new Rect(position.x - 150, position.y - 40, 300, 80), boxStyle);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(selectorNode.nodeName, textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndArea();
    }

    private void DrawSequenceNode(SequenceNode sequenceNode, Vector2 position)
    {
        GUIStyle textStyle = new(EditorStyles.boldLabel)
        {
            fontSize = 16
        };
        GUIStyle boxStyle = new()
        {
            padding = new RectOffset(10, 10, 10, 10),
            normal = { background = Texture2D.grayTexture },
            fontStyle = FontStyle.Bold
        };
        GUILayout.BeginArea(new Rect(position.x - 150, position.y - 40, 300, 80), boxStyle);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(sequenceNode.nodeName, textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndArea();
    }

    private void DrawNode(Node node, Vector2 position)
    {
        if(node.GetType() == typeof(SelectorNode))
        {
            DrawSelectorNode((SelectorNode)node, position);
        }
        else if (node.GetType() == typeof(SequenceNode))
        {
            DrawSequenceNode((SequenceNode)node, position);
        }
    }
}
