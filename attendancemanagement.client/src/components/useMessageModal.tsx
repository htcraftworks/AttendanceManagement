/** npm */
import { type ReactElement, useState, useCallback } from "react";
import { Dialog } from "@mui/material";

/** コンテンツ追加 */
type AdditionalContentProps<ContentProps = undefined>
    = ContentProps extends undefined ? {} : { contentProps?: ContentProps; };

/** コンテンツを描画します。 */
type UseModalOption<ContentProps = undefined> = {
    renderContent: (props: {

        /** props */
        contentProps?: ContentProps | null;

        /** 処理中か */
        processing: boolean;

        /** CLOSEボタンを表示するか */
        hasClose: boolean;

        /** タイトル */
        title: string | null;

        /** メッセージ */
        message: string;

        /** OKアクション */
        ok: () => void;

        /** CLOSEアクション */
        close: () => void;

    }) => ReactElement;
};

/** クリックハンドラー */
type ActionHandler = () => void | Promise<void>;

/** ダイアログ */
type ReturnUseModal<ContentProps = undefined> = {
    confirm: (
        props: {
            title: string;
            message: string;
            onOk?: ActionHandler | undefined;
            onClose?: ActionHandler | undefined;
        } & AdditionalContentProps<ContentProps>
    ) => void;
    /** ダイアログエレメント */
    dialogElement: ReactElement;
};

/**
 * ダイアログを表示します。
 */
const ShowModal = function <ContentProps = undefined>({
    renderContent
}: UseModalOption<ContentProps>): ReturnUseModal<ContentProps> {

    /** ダイアログ表示中か */
    const [isOpen, setIsOpen] = useState<boolean>(false);

    /** 非同期処理中か */
    const [isProcessing, setIsProcessing] = useState<boolean>(false);

    /** キャンセルボタンを表示するか */
    const [hasCancel, setHasClose] = useState<boolean>(false);

    /** ダイアログタイトル */
    const [title, setTitle] = useState<string | null>(null);

    /** ダイアログメッセージ */
    const [message, setMessage] = useState<string>("");

    /** OKボタンクリックアクション */
    const [execOk, setExecOk] = useState<ActionHandler | null>(null);

    /** CLOSEボタンクリックアクション */
    const [execClose, setExecCancel] = useState<ActionHandler | null>(null);

    /** 追加props */
    const [contentProps, setContentProps] = useState<ContentProps | null>();

    /**
     * ハンドル処理
     * @param btnAction アクション
     */
    const handleExecute = async (btnAction: ActionHandler | null) => {

        if (isProcessing) {
            return;
        }

        try {
            if (btnAction == null) {
                return;
            }

            const result = btnAction();

            if (!(result instanceof Promise)) {
                return;
            }

            setIsProcessing(true);
            await result.catch((err) => {
                console.log(err);
            })
        } finally {
            setIsOpen(false);
            setExecOk(null);
            setExecCancel(null);
            setHasClose(false);
            setTitle("");
            setMessage("");
            setIsProcessing(false);
            setContentProps(null);
        }
    };

    /** OKボタンクリックハンドラ */
    const handleOk = useCallback(() => {
        handleExecute(execOk);
    }, [execOk]);

    /** CLOSEボタンクリックハンドラ */
    const handleClose = useCallback(() => {
        handleExecute(execClose);
    }, [execClose]);

    const confirm: ReturnUseModal<ContentProps>["confirm"] = useCallback(
        ({ onOk, onClose, title, message, ...restProps }) => {

            if ("contentProps" in restProps) {
                setContentProps(restProps.contentProps as ContentProps);
            }

            if (onOk) {
                setExecOk(() => onOk);
            }

            if (onClose) {
                setExecCancel(() => onClose);
            }

            setTitle(title);
            setMessage(message);
            setIsOpen(true);
        },
        []
    );

    const dialogContentElement = renderContent({
        contentProps: contentProps,
        processing: isProcessing,
        hasClose: hasCancel,
        title: title,
        message: message,
        ok: handleOk,
        close: handleClose
    });

    const dialogElement: ReturnUseModal<ContentProps>["dialogElement"] = (
        <Dialog
            open={isOpen}
            onClose={handleClose}>
            {dialogContentElement}
        </Dialog>
    );

    return {
        confirm,
        dialogElement
    };
};



/** エクスポート */
export default ShowModal;