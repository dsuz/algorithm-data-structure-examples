このプロジェクトではアルゴリズム・データ構造を活用したゲームプログラミングの例について、Unity を使ったサンプルで説明している。

# プロジェクトのセットアップ

1. リポジトリをクローンするか、zip をダウンロードして展開する
2. プロジェクトを Unity で開く
3. Asset Store から [Space Robot Kyle](https://assetstore.unity.com/packages/3d/characters/robots/4696) をプロジェクトにインポートする

# サンプル

サンプルは /Assets/Examples 以下にあります。

# 各サンプルの説明

## Stack

ウインドウシステムをスタックを使って作っている。新しいウインドウを開く時、それをスタックに Push する。ウインドウを消す時にはスタックから Pop してウインドウを破棄している。この時、スタックの一番上にいるウインドウが次にアクティブウインドウになる。

## Queue

格闘ゲームでよくある「コマンド入力で技を出す」機能をキューを使って作っている。方向キーの入力があると、それを入力キューに保存しておき、攻撃ボタンを押した時にコマンドが入力できているか判定する。技を出すコマンドが入力できていると判定されたら、技が出る。

入力キューは定期的にチェックされ、有効期間を過ぎた入力は随時キューから削除される。