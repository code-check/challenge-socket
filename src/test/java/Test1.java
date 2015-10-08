import java.net.ServerSocket;

public class Test1 extends Thread {
    private ServerSocket server;
    private Main caller;

    public Test1(ServerSocket server, Main caller) {
        this.server = server;
        this.caller = caller;
    }

    @Override
    public void run() {
        try {
            while (server.isClosed()) {
                Thread.sleep(10);
            }
            unlock();
            caller.caughtClient = server.accept();
            unlock();
        } catch (Exception ex) { }
    }

    private void unlock() {
        synchronized (server) {
            server.notify();
        }
    }
}
