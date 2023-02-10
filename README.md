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