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