function criarConexaoSignalR(grupo) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/emprestimohub?grupo=${grupo}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.onclose(error => {
        console.warn(`[SignalR - ${grupo}] Conexão encerrada`, error);
    });

    connection.onreconnecting(error => {
        console.warn(`[SignalR - ${grupo}] Tentando reconectar...`, error);
    });

    connection.onreconnected(connectionId => {
        console.log(`[SignalR - ${grupo}] Reconectado. Connection ID: ${connectionId}`);
    });

    connection.start()
        .then(() => console.log(`[SignalR - ${grupo}] Conectado com sucesso`))
        .catch(err => console.error(`[SignalR - ${grupo}] Erro ao conectar:`, err));

    return connection;
}

window.criarConexaoSignalR = criarConexaoSignalR;
