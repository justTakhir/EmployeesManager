import React, { useState, useEffect } from 'react';
import { Tree, Button, notification, Card, Space, Form, Typography, Input, Col, Row } from 'antd';

function buildFullName(employee) {
    let fullName;
    if (employee == null) {
        fullName = null
    } else {
        fullName = `${employee.firstName} ${employee.middleName} ${employee.lastName}`;
    }

    return fullName;
}

function HireEmployee(props) {
    const [api, contextHolder] = notification.useNotification();
    const [firstName, setFirstName] = useState('');
    const [middleName, setMiddleName] = useState('');
    const [lastName, setLastName] = useState('');
    const [jobTitle, setJobTitle] = useState('');

    let supervisor = props.supervisor;
    let fullName = buildFullName(supervisor);

    function handleHire() {
        let supervisorID = (supervisor != null) ? supervisor.id : null;
        let requestParams;
        if (supervisor != null) {
            requestParams = new URLSearchParams({
                firstName,
                middleName,
                lastName,
                jobTitle,
                supervisorID
            });
        } else {
            requestParams = new URLSearchParams({
                firstName,
                middleName,
                lastName,
                jobTitle
            });
        }

        fetch('/employees/hire?' + requestParams)
            .then((response) => response.json())
            .then((status) => {
                if (status.code != 0) {
                    api['error']({ message: 'Error', description: status.description });
                } else {
                    api['success']({ message: 'OK!', description: 'Сотрудник принят на работу!' });
                    props.onSuccess();
                }
            })
    }

    return (
        <>
            {contextHolder}
            <Card title='Найм сотрудника'>
                <Form labelCol={{ span: 8 }}>
                    <Form.Item label='Имя' required>
                        <Input value={firstName} onChange={(event) => setFirstName(event.target.value)} />
                    </Form.Item>
                    <Form.Item label='Фамилия' required>
                        <Input value={lastName} onChange={(event) => setLastName(event.target.value)} />
                    </Form.Item>
                    <Form.Item label='Отчество' required>
                        <Input value={middleName} onChange={(event) => setMiddleName(event.target.value)} />
                    </Form.Item>
                    <Form.Item label='Должность' required>
                        <Input value={jobTitle} onChange={(event) => setJobTitle(event.target.value)} />
                    </Form.Item>
                    <Form.Item label='Руководитель'>
                        {(supervisor != null) ? fullName : 'выберите руководителя.'}
                    </Form.Item>

                    <Form.Item>
                        <Button onClick={handleHire}>Принять на работу</Button>
                    </Form.Item>
                </Form>
            </Card>
        </>
    )
}

function FireEmployee(props) {
    const [api, contextHolder] = notification.useNotification();
    const [fired, setFired] = useState(null);

    let employee = props.selectedEmployee;
    let fullName = buildFullName(employee);

    function handleNext() {
        props.deselect();
        setFired(employee);
    }

    function selectFired() {
        return (
            <Form layout='vertical'>
                <Form.Item label='Выберите увольняемого сотрудника, кликнув по нему в дереве.'>
                    <Typography.Text strong>{fullName}</Typography.Text>
                </Form.Item>

                <Form.Item>
                    <Button onClick={handleNext}>Далее</Button>
                </Form.Item>
            </Form>
        )
    }

    function handleBack() {
        setFired(null);
    }

    function handleFire() {
        let requestParams;
        if (employee != null) {
            requestParams = new URLSearchParams({
                ID: fired.id,
                newSupervisorID: employee.id
            });
        } else {
            requestParams = new URLSearchParams({
                ID: fired.id,
            });
        }

        fetch('/employees/fire?' + requestParams)
            .then((response) => response.json())
            .then((status) => {
                if (status.code != 0) {
                    api['error']({ message: 'Error', description: status.description })
                } else {
                    api['success']({ message: 'OK!', description: 'Сотрудник успешно уволен!' });
                    props.onSuccess();
                }
            })

        setFired(null);
    }

    function selectNewSupervisor() {
        return (
            <Form layout='vertical'>
                <Form.Item label='Выберите нового руководителя, кликнув по нему в дереве.'>
                    <Typography.Text strong>{fullName}</Typography.Text>
                </Form.Item>

                <Form.Item>
                    <Space>
                        <Button onClick={handleBack}>Назад</Button>
                        <Button onClick={handleFire}>Уволить</Button>
                    </Space>
                </Form.Item>
            </Form>
        )
    }

    return (
        <>
            {contextHolder}
            <Card title='Увольнение сотрудника'>
                {(fired == null) ? selectFired() : selectNewSupervisor()}
            </Card>
        </>
    )
}

function Employee(props) {
    const [api, contextHolder] = notification.useNotification();
    let employee = props.employee;
    let fullName = buildFullName(employee);

    function handleDelete() {
        fetch('/employees/delete')
            .then((response) => response.json())
            .then((status) => {
                if (status.code != 0) {
                    api['error']({ message: 'Error', description: status.description })
                } else {
                    api['success']({ message: 'OK!', description: 'Сотрудник успешно удалён!' });
                }
            })
    }

    return (
        <>
            {contextHolder}
            <Card title={fullName}>
                <p>{employee.jobTitle}</p>
                <p></p>
                <Space>
                    <Button onClick={handleDelete}>Удалить</Button>
                </Space>
            </Card>
        </>
    )
}

function createTree(data, current_key='0') {
    return data.map((e, index) => {
        let next_key = current_key + '-' + index;
        return {
            title: buildFullName(e.employee),
            children: createTree(e.subordinates, next_key),
            key: next_key,
            employee: e.employee
        }
    })
}

export function Home() {
    const [treeData, setTreeData] = useState([]);

    function fetchTreeData() {
        fetch('/employees/getTree')
            .then((response) => response.json())
            .then((data) => {
                let status = data.status;
                let content = data.content;
                if (status.code == 0) {
                    setTreeData(createTree([content]));
                }
            })
    }

    useEffect(() => {
        fetchTreeData();
    }, [])

    const FIVE_SECONDS = 5000;
    useEffect(() => {
        const interval = setInterval(() => {
            fetchTreeData();
        }, FIVE_SECONDS);

        return () => clearInterval(interval);
    }, [])

    const [selectedNode, setSelectedNode] = useState(null);
    const [selectedEmployee, setSelectedEmployee] = useState(null);
    function openEmployee(selectedKeys, e) {
        setSelectedNode(e.node);
        setSelectedEmployee(e.node.employee);
    }

    function deselect() {
        setSelectedNode(null);
        setSelectedEmployee(null);
    }

    return (
        <>
            <Row gutter={[16, 16]}>
                <Col span={8}>
                    <Space direction='vertical'>
                        <Button onClick={deselect}>Снять выделение</Button>
                        <Tree onSelect={openEmployee} selectedKeys={[selectedNode?.key]} treeData={treeData} />
                    </Space>
                </Col>
                <Col span={8}>
                    <Space direction='vertical'>
                        <FireEmployee selectedEmployee={selectedEmployee} onSuccess={fetchTreeData} deselect={deselect} />
                        {(selectedEmployee != null) && <Employee employee={selectedEmployee} />}
                    </Space>
                </Col>
                <Col span={8}>
                    <HireEmployee supervisor={selectedEmployee} onSuccess={fetchTreeData} />
                </Col >

            </Row>
        </>
    );
}
