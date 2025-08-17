/** CSS */
import "../css/Header.css"

/** npm */
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";

/** Consts */
import viewRouteAccountConsts from "../../../consts/viewRouteAccountConsts";
import LocaleSwitcher from "../components/LocaleSwitcher";

/** provider */
import { useAccount } from "../../../provider/AccountProvider";

/** components */
import MessageResourceService from "../../../services/message/messageResourceService";

/**
 * ヘッダー
 * @param userModel ユーザー情報
 * @returns コンポーネント
 */
const Header = () => {

    const { user } = useAccount();
    const msg = new MessageResourceService();

    return (
        <div className="hedder__area">
            <Navbar bg="dark" variant="dark" expand="md" fixed="top">
                <Container fluid className="px-4">
                    <Navbar.Brand className="title" href={viewRouteAccountConsts.Login}>{msg.getMsg("TextAttendanceManagement")}</Navbar.Brand>

                    <Navbar.Toggle aria-controls="basic-navbar-nav" />

                    <Navbar.Collapse id="basic-navbar-nav" className="justify-content-end">
                        <Nav className="ms-auto">
                            {
                                user.userName ?
                                    <NavDropdown className="item" align="end" aria-expanded={true} title={user.userName}>
                                        <NavDropdown.Item href={viewRouteAccountConsts.Logout} >
                                            {msg.getMsg("TextLogout")}
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <LocaleSwitcher />
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                    :
                                    <NavDropdown align="end" title={msg.getMsg("TextMenu")}>
                                        <NavDropdown.Item href={viewRouteAccountConsts.Login}>
                                            {msg.getMsg("TextLogin")}
                                        </NavDropdown.Item>
                                        <NavDropdown.Divider />
                                        <NavDropdown.Item href={viewRouteAccountConsts.UserCreate}>
                                            {msg.getMsg("TextAccountCreate")}
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <LocaleSwitcher />
                                        </NavDropdown.Item>
                                    </NavDropdown>
                            }
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </div>
    )
}

/** エクスポート */
export default Header;