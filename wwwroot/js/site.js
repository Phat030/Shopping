<script>
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();
    
    connection.start().catch(err => console.error(err.toString()));
    
    connection.on('Send', (message) => {
        appendLine(message);
    });

    document.getElementById('frm-send-message').addEventListener('submit', event => {
        let message = document.getElementById('message').value;
        document.getElementById('message').value = '';

        connection.invoke('Send', message);
        event.preventDefault();
    });

    function appendLine(line, color) {
        let li = document.createElement('li');
        li.innerText = line;
        document.getElementById('messages').appendChild(li);
    };

</script>