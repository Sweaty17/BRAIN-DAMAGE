import socket
import threading

port = 65422

def handle_client(conn, addr):
    print(f"Verbindung von {addr} akzeptiert.")
    with conn:
        while True:
            data = conn.recv(1024)
            if not data:
                break
            message = data.decode()
            print(f"Empfangen: {message}")
            if message == "trigger":
                conn.sendall(b'p')
                print("Gesendet: 'p'")

def main():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('localhost', port))
    server_socket.listen(5)
    print(f"Server läuft auf Port {port}. Warte auf Verbindungen...")

    while True:
        conn, addr = server_socket.accept()
        thread = threading.Thread(target=handle_client, args=(conn, addr))
        thread.start()

if __name__ == "__main__":
    main()
