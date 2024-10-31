package DAO;

import model.*;
import Conexao.*;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class AgenciaDAO {
    public void save(Agencia agencia) {
        String sql = "INSERT INTO agencias (numero, endereco, banco_nome) VALUES (?, ?, ?)";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, agencia.getId());
            stmt.setString(2, agencia.getEndereco());
            stmt.setString(3, agencia.getBanco().getNome());
            stmt.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public List<Agencia> findAll() {
        List<Agencia> agencias = new ArrayList<>();
        String sql = "SELECT * FROM agencias";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                int numero = rs.getInt("numero");
                String endereco = rs.getString("endereco");
                String bancoId = rs.getString("banco_id");
                Banco banco = new BancoDAO().findByName(bancoId);
                agencias.add(new Agencia(numero, banco, endereco));
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return agencias;
    }

    public Agencia findById(int id) {
        Agencia agencia = null;
        String sql = "SELECT * FROM agencias WHERE id = ?";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                int numero = rs.getInt("numero");
                String endereco = rs.getString("endereco");
                String bancoId = rs.getString("banco_id");
                Banco banco = new BancoDAO().findByName(bancoId);
                agencia = new Agencia(numero, banco, endereco);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return agencia;
    }

}

