import { PropsWithChildren } from "react";
import { Container } from "react-bootstrap";

type LayoutProps = PropsWithChildren

export const Layout = (props: LayoutProps) => {

    return (<Container>
        {props.children}
    </Container>);

}