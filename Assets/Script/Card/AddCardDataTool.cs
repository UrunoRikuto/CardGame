/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：AddCardDataTool.cs
* 
* 概要：CSVからカードデータを追加するエディタ拡張ツール
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static CardAbility;

/// <summary>
/// CSVからカードデータを追加するエディタ拡張ツール
/// </summary>
public class AddCardDataTool : EditorWindow
{
    // カードデータベースの参照
    private CardDatabase cardDatabase;
    // CSVファイルのパス
    private string csvFilePath = "";

    /// <summary>
    /// メニューに「CustomTools/AddCardData」を追加し、ウィンドウを表示
    /// </summary>
    [MenuItem("CustomTools/AddCardData")]
    public static void ShowWindow()
    {
        GetWindow<AddCardDataTool>("CSVカード追加");
    }

    /// <summary>
    /// GUIの描画
    /// </summary>
    private void OnGUI()
    {
        // タイトル
        GUILayout.Label("CSVからカードデータを追加", EditorStyles.boldLabel);

        // カードデータベースの指定
        cardDatabase = (CardDatabase)EditorGUILayout.ObjectField("Card Database", cardDatabase, typeof(CardDatabase), false);

        // CSVファイルの選択
        if (GUILayout.Button("CSVファイルを選択"))
        {
            // ファイル選択ダイアログを開く
            string path = EditorUtility.OpenFilePanel("CSVファイルを選択", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                csvFilePath = path;
            }
        }

        // 選択されたファイルの表示
        EditorGUILayout.LabelField("選択されたファイル:", string.IsNullOrEmpty(csvFilePath) ? "なし" : csvFilePath);

        // CSVから追加ボタン
        if (GUILayout.Button("CSVから追加"))
        {
            // 入力チェック
            if (cardDatabase == null)
            {
                EditorUtility.DisplayDialog("エラー", "CardDatabase を指定してください。", "OK");
                return;
            }

            // CSVファイルのパスが空の場合
            if (string.IsNullOrEmpty(csvFilePath))
            {
                EditorUtility.DisplayDialog("エラー", "CSVファイルを選択してください。", "OK");
                return;
            }

            // CSVからカードデータを追加
            AddCardsFromCSV(csvFilePath);
        }
    }

    /// <summary>
    /// CSVからカードデータを追加するメソッド
    /// </summary>
    /// <param name="path">ファイルパス</param>
    private void AddCardsFromCSV(string path)
    {
        // CSVファイルの読み込み
        string[] lines = File.ReadAllLines(path);
        // データ行がない場合のエラーチェック
        if (lines.Length < 1)
        {
            EditorUtility.DisplayDialog("エラー", "CSVにデータ行がありません。", "OK");
            return;
        }

        // 新しいカードデータのリスト
        List<CardData> newCards = new List<CardData>();

        // ヘッダー行をスキップしてデータ行を処理
        for (int i = 1; i < lines.Length; i++)
        {
            // 行をカンマで分割
            string[] parts = lines[i].Split(',');

            // 項目数のチェック
            if (parts.Length < 9)
            {
                Debug.LogWarning($"行 {i + 1} は項目数が不足しています。スキップします。");
                continue;
            }

            // 名前, カードの種類, 種族, コスト, 攻撃力, 体力, 効果タイミング, 効果タイプを取得
            string name = parts[0].Trim();
            string type = parts[1].Trim();
            string race = parts[2].Trim();
            string cost = parts[3].Trim();
            string attack = parts[4].Trim();
            string life = parts[5].Trim();
            string effecttiming = parts[6].Trim();
            string effecttype = parts[7].Trim();

            if (cost == "None") cost = "0";
            if (attack == "None") attack = "0";
            if (life == "None") life = "0";

            // CardDataオブジェクトの作成
            CardData card = new CardData
            {
                cardName = name,
                cardType = type,
                cardRace = race,
                cardCost = int.Parse(cost),
                cardAttack = int.Parse(attack),
                cardLife = int.Parse(life),
                cardActivationTiming = CardAbility.SetActivationTiming(effecttiming),
                cardAbilityType = CardAbility.SetAbilityType(effecttype),
            };

            // プレハブの設定が必要な場合はここで設定
            string CardprefabPath = $"Assets/Prefab/CardObject/{name}.prefab";
            GameObject Cardprefab = AssetDatabase.LoadAssetAtPath<GameObject>(CardprefabPath);
            string FieldprefabPath = $"Assets/Prefab/FieldObject/{name}.prefab";
            GameObject Fieldprefab = AssetDatabase.LoadAssetAtPath<GameObject>(FieldprefabPath);
            if (Cardprefab != null)
            {
                card.cardHandPrefab = Cardprefab;

            }
            else
            {
                Debug.LogWarning($"プレハブ '{CardprefabPath}' が見つかりません。カード '{name}' のカードプレハブは設定されません。");
            }
            if (Fieldprefab != null)
            {
                card.cardFieldPrefab = Fieldprefab;
            }
            else
            {
                Debug.LogWarning($"プレハブ '{FieldprefabPath}' が見つかりません。カード '{name}' のフィールドプレハブは設定されません。");
            }

            // 新しいカードデータをリストに追加
            newCards.Add(card);
        }

        // 既存配列に追加
        Undo.RecordObject(cardDatabase, "Add CardData from CSV");

        // 既存のカードデータを取得
        var oldArray = cardDatabase.cardDataBase;
        // 新しいカードデータを追加するためのリストを作成
        var combined = new List<CardData>();
        // 既存のカードデータがあれば追加
        if (oldArray != null) combined.AddRange(oldArray);

        // 既存のカードと同じ名前,属性,種類のカードがある場合は既存データから削除
        foreach (CardData card in newCards.ToArray())
        {
            foreach (CardData existingCard in combined)
            {
                if (existingCard.cardName == card.cardName &&
                    existingCard.cardCost == card.cardCost &&
                    existingCard.cardType == card.cardType)
                {
                    Debug.LogWarning($"カード名 '{card.cardName}' は既に存在します。スキップします。");
                    newCards.Remove(card);
                    break;
                }
            }
        }

        // 新しいカードデータを追加
        combined.AddRange(newCards);

        // 更新された配列をカードデータベースに設定
        cardDatabase.cardDataBase = combined.ToArray();

        EditorUtility.SetDirty(cardDatabase);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("完了", $"{newCards.Count} 件のカードを追加しました。", "OK");
    }
}
