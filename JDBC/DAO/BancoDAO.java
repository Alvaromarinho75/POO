package DAO;

import model.*;
import Conexao.*;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class BancoDAO {
    public void save(Banco banco) {
        String sql = "INSERT INTO bancos (nome) VALUES (?)";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, banco.getNome());
            stmt.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public List<Banco> findAll() {
        List<Banco> bancos = new ArrayList<>();
        String sql = "SELECT * FROM bancos";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                String nome = rs.getString("nome");
                bancos.add(new Banco(nome)); // Adapte conforme necessário
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return bancos;
    }

    public Banco findByName(String nome) {
        Banco banco = null;
        String sql = "SELECT * FROM bancos WHERE nome = ?";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nome);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                String Nome = rs.getString("nome");
                banco = new Banco(Nome);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return banco;
    }

}
