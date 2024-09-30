package app.ewarehouse.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.ComplaintMgmtInspScheduleDocs;

@Repository
public interface ComplaintMgmtInspScheduleDocsRepository extends JpaRepository<ComplaintMgmtInspScheduleDocs, Integer> {

}
