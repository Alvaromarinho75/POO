// filepath: /home/a2023952438/Códigos/T. POO/POO/Rede/Player1/TcpConnection.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Player2;

public class TcpConnection
{
    private TcpListener? listener;
    private TcpClient? client;
    private NetworkStream? stream;

    public void StartServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Servidor iniciado na porta {port}. Aguardando conexão...");
        client = listener.AcceptTcpClient();
        stream = client.GetStream();
        Console.WriteLine("Cliente conectado!");
    }

    public void ConnectToServer(string ip, int port)
    {
        client = new TcpClient();
        client.Connect(IPAddress.Parse(ip), port);
        stream = client.GetStream();
        Console.WriteLine($"Conectado ao servidor {ip}:{port}");
    }

    public void Send(string message)
    {
        if (stream == null) throw new InvalidOperationException("Conexão não estabelecida.");
        var data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    public string Receive()
    {
        if (stream == null) throw new InvalidOperationException("Conexão não estabelecida.");
        var buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    public void StopServer()
    {
        stream?.Close();
        client?.Close();
        listener?.Stop();
        Console.WriteLine("Servidor encerrado.");
    }
}