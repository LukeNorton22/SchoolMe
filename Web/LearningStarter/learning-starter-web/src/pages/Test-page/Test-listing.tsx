import { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import {  Header, Space, Table, createStyles } from "@mantine/core";
import { Container } from "@mantine/core";
import api from "../../config/axios";
import { TestsGetDto, ApiResponse } from "../../constants/types";
import { useNavigate } from "react-router-dom";
import { faTruckMonster } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { routes } from "../../routes";




export const TestListing =  () => {
    const [tests, setTests] = useState<TestsGetDto[]>();
    const navigate = useNavigate();
    const {classes} = useStyles();

    useEffect(() => {
        fetchTests();

        async function fetchTests() { 
            const response = await api.get<ApiResponse<TestsGetDto[]>>("/api/Tests");

            if (response.data.hasErrors) {
                showNotification({ message: "Error fetching Tests." });
            }

            if (response.data.data) {
                setTests(response.data.data);
            }
        }
    }, []); 

    return (
        
        <Container>
        <Header height={32}>Tests</Header>
        <Space h="md" />
        {tests && (
        <Table withBorder striped>
            <thead>
                <tr>
                    <th></th>
                    <th>Test Name</th>
                </tr>
            </thead>
            <tbody>
                {tests.map((tests, index) => {
                    return (
                        <tr key={index}>
                            <td><FontAwesomeIcon
                             className = {classes.iconButton}
                             icon = {faTruckMonster} 
                             onClick={() =>{
                                navigate(
                                    routes.TestUpdate.replace(":id", `${tests.id}`)
                                );
                            }}/></td>
                            <td>{tests.testName}</td>
                            <td>{tests.groupid}</td>
                        </tr>
                    );
                })}
            </tbody>
        </Table>
        )}
    </Container>
    );
};

const useStyles = createStyles(() => {
    return {
        iconButton: {
            cursor: "pointer",
        },
    };
});