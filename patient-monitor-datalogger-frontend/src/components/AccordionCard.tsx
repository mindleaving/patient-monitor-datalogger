import '../styles/accordion-card.css';

import { PropsWithChildren, ReactNode, useEffect, useState } from 'react';
import { Accordion, Card } from 'react-bootstrap';

interface AccordionCardProps extends PropsWithChildren<{}> {
    eventKey: string;
    title: ReactNode;
    collapsedTitle?: ReactNode;
    id?: string;
    className?: string;
    headerClassName?: string;
    bodyClassName?: string;
    bg?: string;
    standalone?: boolean;
    isOpenAtCreate?: boolean;
    mountOnEnter?: boolean;
    onMount?: () => void;
}

export const AccordionCard = (props: AccordionCardProps) => {

    const [ hasBeenMounted, setHasBeenMounted ] = useState<boolean>(!props.mountOnEnter || (props.isOpenAtCreate ?? false));
    const [ isCollapsed, setIsCollapsed ] = useState<boolean>(!props.isOpenAtCreate);

    useEffect(() => {
        if(!isCollapsed) {
            if(!hasBeenMounted) {
                setHasBeenMounted(true);
            }
        }
    }, [ isCollapsed ]);

    useEffect(() => {
        if(hasBeenMounted) {
            if(props.onMount) {
                props.onMount();
            }
        }
    }, [ hasBeenMounted ]);

    const accordionItem = (
        <Accordion.Item
            id={props.id}
            className={props.className}
            eventKey={props.eventKey}
        >
            <Accordion.Button as="div"
                className={props.headerClassName + " clickable"}
                bsPrefix={`accordion-button` + (props.bg ? ` bg-${props.bg}` : '')}
            >
                {isCollapsed && props.collapsedTitle ? props.collapsedTitle : props.title}
            </Accordion.Button>
            <Accordion.Collapse 
                eventKey={props.eventKey}
                onEnter={() => setIsCollapsed(false)}
                onExited={() => setIsCollapsed(true)}
            >
                <Card.Body className={props.bodyClassName ?? 'py-2 px-3'}>
                    {hasBeenMounted ? props.children : null}
                </Card.Body>
            </Accordion.Collapse>
        </Accordion.Item>
    );
    if(!props.standalone) {
        return accordionItem;
    }
    return (
        <Accordion 
            defaultActiveKey={props.isOpenAtCreate ? props.eventKey : undefined}
        >
            {accordionItem}
        </Accordion>
    );

}