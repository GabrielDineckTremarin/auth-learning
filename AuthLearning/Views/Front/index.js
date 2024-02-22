const msgContainer = document.querySelector(".message-container")
const errorMsg= document.querySelector(".error-message")
const successMsg= document.querySelector(".success-message")


document.getElementById("form").addEventListener("submit", function (event) {
    event.preventDefault();
});

const baseURL = 'https://localhost:7208'

async function createUser() {
    const url = baseURL+'/user/create-user';
    
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const passwordConfirmation = document.getElementById('confirmPassword').value;


    const userData = {
        username: username,
        email: email,
        password: password,
        confirmPassword: passwordConfirmation
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        if (responseData.success) {
            // successMsg.innerHTML = responseData.message
            setTimeout(()=> {
                successMsg.innerHTML = ''
                // location.reload()
                location.href = "Login.html"
            },3000)
            
        } else {
            // msgContainer.style.display = 'block'
            // errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                // errorMsg.innerHTML = ''
            },4000)

        }

        
        console.log(responseData); // Handle the response data
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function checkLogin() {
    const url = baseURL+'/user/check-login';
    
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;


    const userData = {
        username: "",
        email: email,
        password: password,
        confirmPassword: ""
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        console.log(responseData)
        if (responseData.success) {
            console.log(responseData.token)
            // successMsg.innerHTML = responseData.message
            localStorage.setItem('token', responseData.token);

   
        } else {
            // msgContainer.style.display = 'block'
            // errorMsg.innerHTML = responseData.message
        }

        
        console.log(responseData); // Handle the response data
    } catch (error) {
        console.error('Fetch error:', error);
    }
}
