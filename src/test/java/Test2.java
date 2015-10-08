import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;

public class Test2 extends Thread {
    private ServerSocket server;
    private Socket client;

    public String message = null;

    public Test2(ServerSocket server, Socket client) {
        this.server = server;
        this.client = client;
    }

    @Override
    public void run() {
        unlock();
        try {
            BufferedReader in = new BufferedReader(new
                    InputStreamReader(client.getInputStream()));
            message = in.readLine();
            unlock();
        } catch (Exception ex) { }
    }

    private void unlock() {
        synchronized (server) {
            server.notify();
        }
    }
}
