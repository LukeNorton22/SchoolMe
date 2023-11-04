import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { ApiResponse, TestsGetDto } from "../../constants/types";
import api from "../../config/axios";

export const TestingPage = () => {
  const { id } = useParams();
  const [test, setTests] = useState<TestsGetDto>();

  useEffect(() => {
    fetchTests();

    async function fetchTests() {
      try {
        const response = await api.get<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`);
        console.log("API Response:", response); // Log the API response

        setTests(response.data.data);
      } catch (error) {
        console.error("Error fetching tests:", error); // Log any errors
      }
    }
  }, [id]);


  return (
    <div>
      {test && (
        <div>
          <h1>Test Name: {test.testName}</h1>
          <ul></ul>
        </div>
      )}
    </div>
  );
};
