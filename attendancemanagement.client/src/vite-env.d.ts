/// <reference types="vite/client" />
/** 環境変数 */
interface ImportMetaEnv {

    /** ベースURL */
    readonly VITE_APP_BASE_URL: string;

    /** ロケール */
    readonly VITE_APP_DEFAULT_LOCALE: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}