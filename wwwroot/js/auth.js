/*
 * Call the Authentication API
 */

export async function SignIn(email, password, redirect) {
    try {
        const response = await fetch('/api/auth/signin', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });

        console.log(`Call '/api/auth/signin'. Status ${response.status}`);
        if (response.ok) {
            if (redirect) {
                location.replace(redirect);
            }
        } else {
            console.error('Failed to sign in');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

export async function SignOut(redirect) {
    try {
        const response = await fetch('/api/auth/signout', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        console.log(`Call '/api/auth/signout'. Status ${response.status}`);
        if (response.ok) {
            if (redirect) {
                location.replace(redirect);
            }
        } else {
            console.error('Failed to sign out');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}
