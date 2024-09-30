package app.ewarehouse.service;

import app.ewarehouse.dto.MFinalOperatorLicenseResponse;
import app.ewarehouse.dto.WarehouseParticularsResponse;
import app.ewarehouse.entity.MFinalOperatorLicense;

import java.util.List;

public interface MFinalOperatorLicenseService {
    WarehouseParticularsResponse findByConformityIdAndBitDeleteFlag(String id);

    List<MFinalOperatorLicense> findAllByBitDeleteFlag();
}
