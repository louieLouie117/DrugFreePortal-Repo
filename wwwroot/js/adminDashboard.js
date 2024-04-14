console.log("adminDashboard.js loaded");

document.addEventListener('DOMContentLoaded', () => {
  fetchAllUsers();
});

// arrow function to fetch all users from the database
const fetchAllUsers = async () => {
    fetch("/GetUsers")
    .then(response => response.json())
    .then(data => {
        console.log("data from db", data);
        console.log("users list", data.usersList);

        RenderAllUsers(data.usersList);
    });
};


const RenderAllUsers = (users) => {
const table = document.getElementById('usersList'); // Assuming you have a table with id 'usersList'
    // Clear the usersList id table
    table.innerHTML = "";

    users.forEach(user => {
        // Map accountType values to descriptive strings
        switch (user.accountType) {
            case 0:
                user.accountType = 'Admin';
                break;
            case 1:
                user.accountType = 'Dean';
                break;
            case 2:
                user.accountType = 'Student';
                break;
            case 3:
                user.accountType = 'Evaluator';
                break;
            default:
                user.accountType = 'Unknown';
        }

        // Create a new table row
        const row = document.createElement('tr');
        row.innerHTML = `
        <td>${user.userId}</td>
        <td>${user.accountType}</td>
        <td>${user.school}</td>
        <td>${user.studentId}</td>
        <td>${user.firstName}</td>
        <td>${user.lastName}</td>
        <td>${user.email}</td>
        <td>${user.password}</td>
        `;

        // Append the row to the table
        table.appendChild(row);
    });
};




window.addEventListener('load', () => {
    fetch('/GetComplianceTypes')
        .then(response => {
            if (response.ok) {
                // Handle successful response
                return response.json();
            } else {
                // Handle error response
                throw new Error('Failed to get compliance types');
            }
        })
        .then(data => {
            // Handle data from backend
            console.log('Data from backend on load:', data);
            // Call a function to handle the data
            ComplianceTypeList(data);
        })
        .catch(error => {
            // Handle network error
            console.error('Network error:', error);
        });
});

// 
const CreateComplianceHandler = (event) => {
    event.preventDefault();

    const SchoolsSelector = document.getElementById("SchoolsSelector").value;
    if (SchoolsSelector === "Select School") {
        alert("Please select a school");
        return;
    }
    const schoolId = document.getElementById("SchoolId").value;
    const complianceName = document.getElementById("ComplianceName").value;
    const complianceDetails = document.getElementById("ComplianceDetails").value;


    const data = {
        school: SchoolsSelector,
        schoolId: schoolId,
        name: complianceName,
        details: complianceDetails
    };
    console.log(data);

    fetch('/AddCompliance', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                // Handle successful response
                console.log('Compliance created successfully');
                return response.json();
            } else {
                // Handle error response
                console.error('Failed to create compliance');
                throw new Error('Failed to create compliance');
            }
        })
        .then(data => {
            // Handle data from backend
            console.log('Data from backend after submiting:', data);
            // Call a function to handle the data
            ComplianceTypeList(data);
        })
        .catch(error => {
            // Handle network error
            console.error('Network error:', error);
        });
};




const ComplianceTypeList = (data) => {
    console.log("data to loop", data.data);
    const complianceTypeList = document.getElementById("ComplianceTypeList");
    complianceTypeList.innerHTML = "";
    data.data.forEach(compliance => {
        console.log("compliance", compliance);
     
       
        // Create table body
        let row = `
            <tr>
                <td>${compliance.name}</td>
                <td>${compliance.school}</td>
                <td>${compliance.schoolId}</td>
                <td>${compliance.details}</td>
            </tr>
        `;

        // Append the table body to the complianceTypeList
        complianceTypeList.innerHTML += row;
    });
};