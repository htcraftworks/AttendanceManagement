import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// 基底フォルダパスを特定（APPDATE=Windows,HOME=Linux）
const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "attendancemanagement.client";

// 証明書ファイルパス
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);

// pemキーファイルパス
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

// 証明書ファイルとpemキーファイルが存在する場合は
// dotnet dev-certs コマンドで、ローカル Web アプリの HTTPS を有効にする
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

/** サーバーエンド側のポート7038 */
const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7038';

// https://vitejs.dev/config/
export default defineConfig({
    // vitejs/plugin-react プラグインを使用する
    plugins: [plugin()],
    // パスエイリアスを設定する（@で./srcにアクセスできるようにする）
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url)),
            '@mui/styled-engine': '@mui/styled-engine'
        }
    },
    server: {
        proxy: {

            //Hack:記載順に適用される
            // アカウント機能API
            '^/account': {
                target,
                changeOrigin: true,
                secure: false
            },
            // メニュー機能API
            '^/mainmenu': {
                target,
                changeOrigin: true,
                secure: false,
            },
        },
        // サーバーのポートを指定する
        port: parseInt(env.DEV_SERVER_PORT || '51100'),

        // TLS + HTTP/2 を有効にする（証明書ファイルとpemキーファイルを指定）
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
