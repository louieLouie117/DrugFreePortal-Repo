console.log('reg.js is loaded');


const getWorkingURL = () => {
    const runningUrl = window.location.href;
    console.log('runningUrl', runningUrl);
    let localDevelopmentUrl = 'http://localhost:5043/';
    let developemtServerUrl = 'http://13.58.200.202/';
    let cleintSeverUrl = 'http://3.135.214.156/';

    let workingURL = "";

    if (runningUrl === localDevelopmentUrl) {
        workingURL = localDevelopmentUrl + "LoginFetch";
    } else if (runningUrl === developemtServerUrl) {
        workingURL = developemtServerUrl + "LoginFetch";
    } else if (runningUrl === cleintSeverUrl) {
        workingURL = cleintSeverUrl + "LoginFetch";
    }

    return workingURL;
}


const signInHandlerFetch = async (e) => {
    e.preventDefault();

    const workingURL = getWorkingURL();
    // Rest of the code...
    console.log('workingURL SignInhandlerFetch', workingURL);
    // Get the email and password from input fields
    const email = document.getElementById('signInemail').value;
    const password = document.getElementById('signInPassword').value;

    // Construct the complete URL

    // Create an object with the email and password
    const jsonData = {
        email: email,
        password: password
    };
    console.log('jsonData', jsonData);



        fetch(`${workingURL}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonData)
        })
            .then(response => response.json())
            .then(data => {
                // Handle the response data
                console.log(data);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });

};




