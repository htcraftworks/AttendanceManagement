# 🕘 勤怠管理アプリケーション（Attendance Management App）

本アプリは、**ASP.NET Core** と **React + TypeScript** を使用したフルスタックの勤怠管理システムです。  
出勤・退勤・休憩時間などの登録、勤怠一覧の表示・編集、アカウント作成や認証（JWT）、CORS設定をサポートしています。

ログイン・ログアウト、アカウント管理、勤怠の出退勤登録や編集、一覧確認などの機能を備えています。  
認証は **ASP.NET Core Identity + JWT トークン**、データベースは **Entity Framework Core (Code First)** によるマイグレーション方式で構築しています。  
さらに、**独自リソースファイルによる多言語対応**を備えています。
フロントエンドはレスポンシブデザイン、ダークモードにも対応しています。

---

## ✅ 機能一覧

### 🔐 ユーザー認証
- ログイン／ログアウト
- アカウント作成（ユーザID・パスワード）

### 🕘 勤怠管理
- 出勤登録／退勤登録
- 勤怠一覧の表示
- 勤怠編集（勤務時間・休憩時間・勤務形態・変更理由）
- 勤怠削除（削除理由）

### 🌐 共通機能
- 多言語対応（一部リソースベース）
- ローカル／セッションストレージによる状態管理（※フロントエンド側）
- `.env` / `appsettings.json` による環境設定

---

## 🚀 技術スタック

### 🖥️ バックエンド

- **フレームワーク**：ASP.NET Core（.NET 8）
- **ORマッパー**：Entity Framework Core（Code First）
- **認証・認可**：
  - ASP.NET Identity  
  - JWT 認証
- **API**：RESTful API 設計
- **多言語対応**：独自 JSON リソースベースによる実装
- **環境設定**：`appsettings.json` による環境ごとの設定管理
- **コード品質管理**：StyleCop.Analyzers によるコーディング規約の統一と静的解析
- **openAPI仕様書**：Swaggerによるコードファースト仕様書作成

---

### 💻 フロントエンド

- **フレームワーク**：React（TypeScript + Vite）
- **UIライブラリ**：
  - Bootstrap（react-bootstrap）
  - Material UI（MUI）
- **ルーティング**：React Router DOM
- **日付選択コンポーネント**：React Datepicker
- **フォームバリデーション**：Formik + Yup
- **API通信**：Axios
- **多言語対応**：独自実装による多言語対応機能
- **ストレージ管理**：
  - ローカルストレージ
  - セッションストレージ
- **環境設定**：`.env` による環境変数管理（Vite）
- **コード品質管理**：Eslint によるコーディング規約の統一と静的解析

---

## 🖥 実行環境

### 共通環境

- **Visual Studio 2022** Version: `17.14.9`
- **Node.js** Version: `v22.18.0`

---

### バックエンド

#### 実行環境

- **.NET SDK**: `8.x`

#### NuGet パッケージ

- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: `9.0.7`
- **Microsoft.Extensions.Configuration.Json**: `9.0.7`
- **Microsoft.AspNetCore.Authentication.JwtBearer**: `9.0.7`
- **Microsoft.AspNetCore.SpaProxy**: `9.0.8`
- **Microsoft.EntityFrameworkCore.Design**: `9.0.8`
- **Microsoft.EntityFrameworkCore.SqlServer**: `9.0.7`
- **Microsoft.EntityFrameworkCore.Tools**: `9.0.8`
- **stylecop.analyzers**: `1.1.118`
- **swashbuckle.aspnetcore**: `9.0.3`
- **swashbuckle.aspnetcore.annotations**: `9.0.3`

---

### フロントエンド

#### 実行環境

- **React**: `^19.1.1`
- **React DOM**: `^19.1.1`

#### 開発用パッケージ

- **ESLint**: `^9.32.0`
- **TypeScript**: `~5.8.3`
- **Vite**: `^7.1.0`
- **@vitejs/plugin-react**: `^4.7.0`
- **@eslint/js**: `^9.32.0`
- **@types/react**: `^19.1.9`
- **@types/react-dom**: `^19.1.7`
- **eslint-plugin-react-hooks**: `^5.2.0`
- **eslint-plugin-react-refresh**: `^0.4.20`
- **globals**: `^16.3.0`
- **typescript-eslint**: `^8.39.0`
- **@types/node**: `^22`

#### ライブラリ

- **@emotion/react**: `^11.14.0`
- **@emotion/styled**: `^11.14.1`
- **@mui/material**: `^7.3.1`
- **axios**: `^1.11.0`
- **bootstrap**: `^5.3.7`
- **formik**: `^2.4.6`
- **react-bootstrap**: `^2.10.10`
- **react-datepicker**: `^8.4.0`
- **react-router-dom**: `^7.8.0`
- **yup**: `^1.7.0`

---

## 🧪 開発環境構築手順

### 共通

1. 必要に応じて以下をインストールしてください

   ```
   Visual Studio
   Node.js
   ```

2. バックエンド、フロントエンド両方起動する場合は、マルチスタートアップを設定してください。

### バックエンド

1. Visual Studio でソリューションを開く  
   ```
   ./AttendanceManagement.sln
   ```
   

2. データベースを構築する（初回のみ）  
   Visual Studio のパッケージマネージャー コンソールで以下を実行：

   ```powershell
   Update-Database InitCreate -Project Data.DbContexts -StartupProject AttendanceManagement.Server
   ```

   マイグレーションファイル：  
   ```
   ./Database/DbContexts/Migrations/
   ```

3. `AttendanceManagement.Server` プロジェクトを HTTPS で起動  
   起動設定ファイル：  
   ```
   ./AttendanceManagement.Server/Properties/launchSettings.json
   ```

### フロントエンド

1. Visual Studio でソリューションファイルを開きます：

   ```bash
   ../AttendanceManagement.sln
   ```

2. 必要な NPM パッケージをインストールします（`./attendancemanagement.client` ディレクトリで実行）：

   ```bash
   npm install react-bootstrap bootstrap
   npm install react-datepicker
   npm install react-router-dom
   npm install @mui/material
   npm install formik
   npm install axios
   npm install yup
   ```

3. `./attendancemanagement.client` プロジェクトを Chrome 等のブラウザで起動します。

   - 起動設定ファイル:  
     `./attendancemanagement.client/.vscode/launch.json`

   - Vite によるプロキシ設定あり（サーバーとのリダイレクト通信）  
     設定ファイル: `./attendancemanagement.client/vite.config.ts`（60行目付近proxy:）

---

## 📁 ディレクトリ構成

### バックエンド

```plaintext
./AttendanceManagement.Server/               # バックエンドのメインプロジェクト（Web API）
├── Controllers/                            # Web APIコントローラー群
│   └── WebApiControllers/                 # 認証・勤怠などのAPIエンドポイント
│
├── Properties/                            # プロジェクト起動設定（launchSettings.json）
│
├── Services/                              # サーバーサイドのビジネスロジック（JWT認証など）

./Common/                                     # 共通定数やロギング関連（全体で利用）
└── Const/                                 # 定数やロガーコードの定義
    └── class/                             # 定数クラスの実装

./Database/                                   # データベースアクセス層
├── Accessor/                              # DBアクセス処理（リポジトリ層）
│   ├── Param/                             # DBアクセス用の入力パラメータ
│   └── Result/                            # DBアクセスからの返却データ構造
│
├── DbContexts/                            # EF CoreのDbContext定義とマイグレーション
│   ├── Context/                           # DbContext本体とファクトリ
│   └── Migrations/                        # マイグレーションファイル一式（コードファースト）
│
└── Entities/                              # データベースエンティティ（モデル）
    ├── Entities/                          # テーブル定義に対応するクラス
    └── Model/                             # API用レスポンスモデルなどの補助モデル

./Library/                                    # 汎用ライブラリ（日時ユーティリティ、ログ出力など）

./WebApiService/                              # Web API のサービスレイヤー
├── Common/                                # APIで使用される定数や列挙型
│   └── class/                             # 各定数・列挙型の詳細クラス
│
├── Logic/                                 # 業務ロジック（アカウント・勤怠など）
│   ├── Logic/                             # ロジック本体
│   └── Model/                             # 各ロジック用のParam・Resultモデル
│       ├── Param/                         # 入力モデル
│       └── Result/                        # 出力モデル
│
└── Resource/                              # 多言語リソース（resx形式）
```

### フロントエンド

```plaintext
./attendancemanagement.client/
├── public/                     # 静的リソース（画像・静的TSファイルなど）
│   ├── images/                 # ロゴやアイコンなど
│   └── resource/               # メッセージリソース等
│       └── types/              # リソース型定義
│
├── src/                        # アプリ本体
│   ├── components/             # 汎用コンポーネント（Button, Inputなど）
│   │   └── css/                # 各コンポーネントのCSS
│   │
│   ├── consts/                 # 共通定数、ルート定義、HTTPコードなど
│   │   └── types/              # 定数で使用する型定義
│   │
│   ├── features/               # 各画面・機能ごとの構成
│   │   ├── account/            # ログイン・アカウント作成画面
│   │   │   ├── logic/          # アカウント用ロジック
│   │   │   └── types/          # 型定義
│   │   ├── attendance/         # 勤怠登録・編集・一覧
│   │   │   ├── logic/          # 勤怠ロジック
│   │   │   ├── css/            # 勤怠画面スタイル
│   │   │   └── types/          # モデル定義
│   │   └── App/                # 全体構成（ヘッダー、フッター、ルーティング等）
│   │       ├── components/     # App共通UIパーツ
│   │       ├── css/            # レイアウトや全体スタイル
│   │       ├── logic/          # App共通ロジック
│   │       └── types/          # Appで使用する型
│   │
│   ├── provider/               # Context APIによる状態管理（認証など）
│   │   └── types/              # Contextの型
│   │
│   ├── router/                 # React Router定義
│   │
│   ├── services/               # 外部サービスとの連携・ストレージ
│   │   ├── api/                # API通信
│   │   │   ├── param/          # APIリクエスト型
│   │   │   └── result/         # APIレスポンス型
│   │   ├── message/            # 多言語リソース取得
│   │   └── storage/            # ローカル/セッションストレージ操作
│   │
│   ├── types/                  # 共通の型（ユーザー、辞書等）
│   └── utils/                  # 汎用ユーティリティ関数
│       └── types/              # ユーティリティ関連の型
│
├── .env                        # 環境変数定義（Vite）
├── vite.config.ts              # Vite設定ファイル
├── tsconfig.json               # TypeScript設定
├── package.json                # 使用パッケージ定義
└── README.md                   # このファイル
```

---

## API仕様書

### バックエンド

本プロジェクトのバックエンドAPI仕様書は、Swagger UIで公開されています。
APIの各エンドポイントの詳細な仕様やリクエスト・レスポンスのフォーマットをブラウザ上で確認できます：

以下のリンクからアクセスしてください：

[AttendanceManagement バックエンド API Swagger UI](https://attendancemanagementserver20250815185416-b3htguhjgyfvafg0.japanwest-01.azurewebsites.net/swagger)

---

## 🌐 デモサイト（動作確認用）

以下のリンクから、アプリケーションの動作を確認できます：

🔗 **[勤怠管理アプリ デモサイト](https://attendancemanagementserver20250815185416-b3htguhjgyfvafg0.japanwest-01.azurewebsites.net/attendancemenu)**  
（※ログイン後、勤怠メニュー画面へ遷移します）

---

### ⚠️ 注意事項

- デモサイトは動作確認用であり、**登録データは定期的にリセット**されます。
- レスポンスや動作速度は、**Azure 無料プラン**上のホスティング環境による制限を受けます。

---

## 備忘録

1. EF Core操作

-  Migration追加

   ```bash
   Add-Migration InitCreate -Project Data.DbContexts -StartupProject AttendanceManagement.Server
   ```

-  Migration削除

   ```bash
   Remove-Migration -Project Data.DbContexts -StartupProject AttendanceManagement.Server
   ```

---

## 📎 ライセンス

このプロジェクトは **ポートフォリオ目的** での利用に限定されており、再配布・商用利用を禁止しています。

詳細は [`./LICENSE.txt`](./LICENSE.txt) をご確認ください。

---
