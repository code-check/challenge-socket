
//BEGIN_CHALLENGE
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.Socket;
//END_CHALLENGE
public class Client implements IClient  {
    //BEGIN_CHALLENGE
    private Socket socket;
    private DataOutputStream out;

    public void connect(String ip, Integer port) {
        try {
            socket = new Socket(ip, port);
            out = new DataOutputStream(socket.getOutputStream());
            out.writeBytes("Hello World!\n");
            BufferedReader in = new BufferedReader(new
                    InputStreamReader(socket.getInputStream()));
            while (true) {
                if (in.readLine().equals("Marco")) {
                    out.writeBytes("Polo\n");
                }
            }
        } catch (Exception ex) { }
    }
    //END_CHALLENGE
}
