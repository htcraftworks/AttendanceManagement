/** CSS */
import "./css/MessageModal.css"

/** npm */
import { Button, DialogTitle, DialogContent, DialogActions, Typography } from "@mui/material";

/** consts */
/** components */
/** types */
import ShowModal from "./useMessageModal";

/**
 * ダイアログ
 * @returns コンポーネント
 */
const MessageModal = () => {
    return ShowModal<{ hasClose: boolean }>({
        renderContent: ({ contentProps, title, message, ok, close }) => {
            return (
                <>
                    {message &&
                        <div>
                            <DialogTitle>{title}</DialogTitle>
                            <DialogContent className="dialog-content">
                                <Typography>
                                    {message}
                                </Typography>
                            </DialogContent>
                            <DialogActions>
                                <Button variant="outlined" hidden={contentProps ? !contentProps.hasClose : true} onClick={close} >
                                    キャンセル
                                </Button>
                                <Button variant="contained" onClick={ok} >
                                    OK
                                </Button>
                            </DialogActions>
                        </div>
                    }
                </>
            );
        }
    });
};

/** エクスポート */
export default MessageModal;